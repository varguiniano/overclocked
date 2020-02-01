using UnityEngine;
using UnityEngine.UI;

namespace Varguiniano.Core.Runtime.Gui
{
    /// <summary>
    /// Mono behaviour that performs an action when the button it's attached to is clicked.
    /// </summary>
    [RequireComponent(typeof(Button))]
    public abstract class ActionOnButtonClick : MonoBehaviour
    {
        /// <summary>
        /// Reference to the button.
        /// </summary>
        private Button button;

        /// <summary>
        /// Subscribe.
        /// </summary>
        protected void OnEnable()
        {
            button = GetComponent<Button>();
            button.onClick.AddListener(ButtonClicked);
        }

        /// <summary>
        /// Unsubscribe.
        /// </summary>
        protected void OnDisable() => button.onClick.RemoveListener(ButtonClicked);

        /// <summary>
        /// Called when the button is clicked.
        /// </summary>
        protected abstract void ButtonClicked();
    }
}