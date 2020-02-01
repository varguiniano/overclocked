using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Varguiniano.Core.Runtime.Localization;

namespace Varguiniano.Core.Runtime.Gui
{
    /// <summary>
    /// Class that handles a language selection dropdown.
    /// </summary>
    [RequireComponent(typeof(TMP_Dropdown))]
    public class LanguageSelection : MonoBehaviour
    {
        /// <summary>
        /// Reference to the dropdown.
        /// </summary>
        private TMP_Dropdown dropdown;

        /// <summary>
        /// Initialize the dropdown.
        /// </summary>
        private void OnEnable()
        {
            dropdown = GetComponent<TMP_Dropdown>();

            RefreshOptions();

            dropdown.onValueChanged.AddListener(value =>
                                                {
                                                    LocalizationManager.Instance.CurrentLanguage = (uint) value;
                                                    RefreshOptions();
                                                });
        }

        /// <summary>
        /// Deinitialize.
        /// </summary>
        private void OnDisable() => dropdown.onValueChanged.RemoveAllListeners();

        /// <summary>
        /// Refresh the dropdown options.
        /// </summary>
        private void RefreshOptions()
        {
            List<TMP_Dropdown.OptionData> options = new List<TMP_Dropdown.OptionData>();

            List<LocalizableString> optionList = LocalizationManager.Instance.LanguageLibrary.LanguageNames;

            for (int i = 0; i < optionList.Count; i++)
                options.Add(new TMP_Dropdown.OptionData(optionList[i].LocalizedValue));

            dropdown.options = options;
            dropdown.value = (int) LocalizationManager.Instance.CurrentLanguage;
        }
    }
}