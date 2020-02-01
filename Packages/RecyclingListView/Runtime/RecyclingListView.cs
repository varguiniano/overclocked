using System;
using UnityEngine;
using UnityEngine.UI;

namespace Sinbad.UnityRecyclingListView
{
    /// <summary>
    /// RecyclingListView uses a Unity UI ScrollRect to provide an efficiently scrolling list.
    /// The key feature is that it only allocates just enough child items needed for the
    /// visible part of the view, and recycles them as the list is scrolled, saving memory
    /// and layout cost.
    ///
    /// There are limitations:
    ///   * Child items must be a fixed height
    ///   * Only one type of child item is supported
    ///   * Only vertical scrolling is virtualised. Horizontal scrolling is still supported but
    ///     there is no support for grid view style layouts 
    /// </summary>
    [RequireComponent(typeof(ScrollRect))]
    // ReSharper disable once ClassWithVirtualMembersNeverInherited.Global
    public class RecyclingListView : MonoBehaviour
    {
        [Tooltip("Prefab for all the child view objects in the list")]
        public RecyclingListViewItem ChildPrefab;

        [Tooltip("The amount of vertical padding to add between items")]
        public float RowPadding = 15f;

        [Tooltip("Minimum height to pre-allocate list items for. Use to prevent allocations on resizing.")]
        public float PreAllocHeight;

        /// <summary>
        /// Set the vertical normalized scroll position. 0 is bottom, 1 is top (as with ScrollRect) 
        /// </summary>
        public float VerticalNormalizedPosition
        {
            get => ScrollRect.verticalNormalizedPosition;
            set => ScrollRect.verticalNormalizedPosition = value;
        }

        // ReSharper disable once InconsistentNaming
        protected int rowCount;

        /// <summary>
        /// Get / set the number of rows in the list. If changed, will cause a rebuild of
        /// the contents of the list. Call Refresh() instead to update contents without changing
        /// length.
        /// </summary>
        public int RowCount
        {
            get => rowCount;
            set
            {
                if (rowCount != value)
                {
                    rowCount = value;
                    // avoid triggering double refresh due to scroll change from height change
                    IgnoreScrollChange = true;
                    UpdateContentHeight();
                    IgnoreScrollChange = false;
                    ReorganiseContent(true);
                }
            }
        }

        /// <summary>
        /// Delegate which users should implement to populate their custom RecyclingListViewItem
        /// instances when they're needed by the list.
        /// </summary>
        /// <param name="item">The child item being populated. These are recycled as the list scrolls.</param>
        /// <param name="rowIndex">The overall row index of the list item being populated</param>
        public delegate void ItemDelegate(RecyclingListViewItem item, int rowIndex);

        /// <summary>
        /// Set the delegate which will be called back to populate items. You must provide this at runtime.
        /// </summary>
        public ItemDelegate ItemCallback;

        protected ScrollRect ScrollRect;

        // circular buffer of child items which are reused
        protected RecyclingListViewItem[] ChildItems;

        // the current start index of the circular buffer
        protected int ChildBufferStart;

        // the index into source data which childBufferStart refers to 
        protected int SourceDataRowStart;

        protected bool IgnoreScrollChange;
        protected float PreviousBuildHeight;
        protected const int RowsAboveBelow = 1;

        /// <summary>
        /// Trigger the refreshing of the list content (e.g. if you've changed some values).
        /// Use this if the number of rows hasn't changed but you want to update the contents
        /// for some other reason. All active items will have the ItemCallback invoked. 
        /// </summary>
        public virtual void Refresh()
        {
            ReorganiseContent(true);
        }

        /// <summary>
        /// Refresh a subset of the list content. Any rows which currently have data populated in the view
        /// will cause a call to ItemCallback. The size of the list or positions won't change.
        /// </summary>
        /// <param name="rowStart"></param>
        /// <param name="count"></param>
        public virtual void Refresh(int rowStart, int count)
        {
            // only refresh the overlap
            int sourceDataLimit = SourceDataRowStart + ChildItems.Length;

            for (int i = 0; i < count; ++i)
            {
                int row = rowStart + i;
                if (row < SourceDataRowStart || row >= sourceDataLimit) continue;

                int bufIdx = WrapChildIndex(ChildBufferStart + row - SourceDataRowStart);

                if (ChildItems[bufIdx] != null)
                {
                    UpdateChild(ChildItems[bufIdx], row);
                }
            }
        }

        /// <summary>
        /// Refresh a single row based on its reference.
        /// </summary>
        /// <param name="item"></param>
        public virtual void Refresh(RecyclingListViewItem item)
        {
            for (int i = 0; i < ChildItems.Length; ++i)
            {
                int idx = WrapChildIndex(ChildBufferStart + i);

                if (ChildItems[idx] != null && ChildItems[idx] == item)
                {
                    UpdateChild(ChildItems[i], SourceDataRowStart + i);
                    break;
                }
            }
        }

        /// <summary>
        /// Quick way of clearing all the content from the list (alias for RowCount = 0)
        /// </summary>
        public virtual void Clear()
        {
            RowCount = 0;
        }

        /// <summary>
        /// Scroll the viewport so that a given row is in view, preferably centred vertically.
        /// </summary>
        /// <param name="row"></param>
        public virtual void ScrollToRow(int row)
        {
            ScrollRect.verticalNormalizedPosition = GetRowScrollPosition(row);
        }

        /// <summary>
        /// Get the normalised vertical scroll position which would centre the given row in the view,
        /// as best as possible without scrolling outside the bounds of the content.
        /// Use this instead of ScrollToRow if you want to control the actual scrolling yourself.
        /// </summary>
        /// <param name="row"></param>
        /// <returns></returns>
        public float GetRowScrollPosition(int row)
        {
            float rowCentre = (row + 0.5f) * RowHeight();
            float vpHeight = ViewportHeight();
            float halfVpHeight = vpHeight * 0.5f;
            // Clamp to top of content
            float vpTop = Mathf.Max(0, rowCentre - halfVpHeight);
            float vpBottom = vpTop + vpHeight;
            float contentHeight = ScrollRect.content.sizeDelta.y;

            // clamp to bottom of content
            if (vpBottom > contentHeight) // if content is shorter than vp always stop at 0
                vpTop = Mathf.Max(0, vpTop - (vpBottom - contentHeight));

            // Range for our purposes is between top (0) and top of vp when scrolled to bottom (contentHeight - vpHeight)
            // ScrollRect normalised position is 0 at bottom, 1 at top
            // so inverted range because 0 is bottom and our calc is top-down
            return Mathf.InverseLerp(contentHeight - vpHeight, 0, vpTop);
        }

        protected virtual void Awake()
        {
            ScrollRect = GetComponent<ScrollRect>();
        }

        protected virtual bool CheckChildItems()
        {
            float vpHeight = ViewportHeight();
            float buildHeight = Mathf.Max(vpHeight, PreAllocHeight);
            bool rebuild = ChildItems == null || buildHeight > PreviousBuildHeight;

            if (rebuild)
            {
                // create a fixed number of children, we'll re-use them when scrolling
                // figure out how many we need, round up
                int childCount = Mathf.RoundToInt(0.5f + buildHeight / RowHeight());
                childCount += RowsAboveBelow * 2; // X before, X after

                if (ChildItems == null)
                    ChildItems = new RecyclingListViewItem[childCount];
                else if (childCount > ChildItems.Length) Array.Resize(ref ChildItems, childCount);

                for (int i = 0; i < ChildItems.Length; ++i)
                {
                    if (ChildItems[i] == null)
                    {
                        ChildItems[i] = Instantiate(ChildPrefab);
                    }

                    ChildItems[i].RectTransform.SetParent(ScrollRect.content, false);
                    ChildItems[i].gameObject.SetActive(false);
                }

                PreviousBuildHeight = buildHeight;
            }

            return rebuild;
        }

        protected virtual void OnEnable()
        {
            ScrollRect.onValueChanged.AddListener(OnScrollChanged);
            IgnoreScrollChange = false;
        }

        protected virtual void OnDisable()
        {
            ScrollRect.onValueChanged.RemoveListener(OnScrollChanged);
        }

        protected virtual void OnScrollChanged(Vector2 normalisedPos)
        {
            // This is called when scroll bar is moved *and* when viewport changes size
            if (!IgnoreScrollChange)
            {
                ReorganiseContent(false);
            }
        }

        protected virtual void ReorganiseContent(bool clearContents)
        {
            if (clearContents)
            {
                ScrollRect.StopMovement();
                ScrollRect.verticalNormalizedPosition = 1; // 1 == top
            }

            bool childrenChanged = CheckChildItems();
            bool populateAll = childrenChanged || clearContents;

            // Figure out which is the first virtual slot visible
            float ymin = ScrollRect.content.localPosition.y;

            // round down to find first visible
            int firstVisibleIndex = (int) (ymin / RowHeight());

            // we always want to start our buffer before
            int newRowStart = firstVisibleIndex - RowsAboveBelow;

            // If we've moved too far to be able to reuse anything, same as init case
            int diff = newRowStart - SourceDataRowStart;

            if (populateAll || Mathf.Abs(diff) >= ChildItems.Length)
            {
                SourceDataRowStart = newRowStart;
                ChildBufferStart = 0;
                int rowIdx = newRowStart;

                foreach (var item in ChildItems)
                {
                    UpdateChild(item, rowIdx++);
                }
            }
            else if (diff != 0)
            {
                // we scrolled forwards or backwards within the tolerance that we can re-use some of what we have
                // Move our window so that we just re-use from back and place in front
                // children which were already there and contain correct data won't need changing
                int newBufferStart = (ChildBufferStart + diff) % ChildItems.Length;

                if (diff < 0)
                {
                    // window moved backwards
                    for (int i = 1; i <= -diff; ++i)
                    {
                        int bufi = WrapChildIndex(ChildBufferStart - i);
                        int rowIdx = SourceDataRowStart - i;
                        UpdateChild(ChildItems[bufi], rowIdx);
                    }
                }
                else
                {
                    // window moved forwards
                    int prevLastBufIdx = ChildBufferStart + ChildItems.Length - 1;
                    int prevLastRowIdx = SourceDataRowStart + ChildItems.Length - 1;

                    for (int i = 1; i <= diff; ++i)
                    {
                        int bufi = WrapChildIndex(prevLastBufIdx + i);
                        int rowIdx = prevLastRowIdx + i;
                        UpdateChild(ChildItems[bufi], rowIdx);
                    }
                }

                SourceDataRowStart = newRowStart;
                ChildBufferStart = newBufferStart;
            }
        }

        private int WrapChildIndex(int idx)
        {
            while (idx < 0) idx += ChildItems.Length;

            return idx % ChildItems.Length;
        }

        private float RowHeight()
        {
            return RowPadding + ChildPrefab.RectTransform.rect.height;
        }

        private float ViewportHeight()
        {
            return ScrollRect.viewport.rect.height;
        }

        protected virtual void UpdateChild(RecyclingListViewItem child, int rowIdx)
        {
            if (rowIdx < 0 || rowIdx >= rowCount)
            {
                // Out of range of data, can happen
                child.gameObject.SetActive(false);
            }
            else
            {
                if (ItemCallback == null)
                {
                    Debug.Log("RecyclingListView is missing an ItemCallback, cannot function", this);
                    return;
                }

                // Move to correct location
                var childRect = ChildPrefab.RectTransform.rect;
                Vector2 pivot = ChildPrefab.RectTransform.pivot;
                float ytoppos = RowHeight() * rowIdx;
                float ypos = ytoppos + (1f - pivot.y) * childRect.height;
                float xpos = 0 + pivot.x * childRect.width;
                child.RectTransform.anchoredPosition = new Vector2(xpos, -ypos);
                child.NotifyCurrentAssignment(this, rowIdx);

                // Populate data
                ItemCallback(child, rowIdx);

                child.gameObject.SetActive(true);
            }
        }

        protected virtual void UpdateContentHeight()
        {
            float height = ChildPrefab.RectTransform.rect.height * rowCount + (rowCount - 1) * RowPadding;
            // apparently 'sizeDelta' is the way to set w / h 
            var content = ScrollRect.content;
            var sz = content.sizeDelta;
            content.sizeDelta = new Vector2(sz.x, height);
        }

        protected virtual void DisableAllChildren()
        {
            if (ChildItems != null)
            {
                for (int i = 0; i < ChildItems.Length; ++i)
                {
                    ChildItems[i].gameObject.SetActive(false);
                }
            }
        }
    }
}