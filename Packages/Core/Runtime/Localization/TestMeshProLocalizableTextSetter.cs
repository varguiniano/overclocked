using TMPro;
using UnityEngine;

namespace Varguiniano.Core.Runtime.Localization
{
    /// <summary>
    /// Class that sets the component TMP_Text with the corresponding localizable string.
    /// </summary>
    [RequireComponent(typeof(TMP_Text))]
    public class TestMeshProLocalizableTextSetter : MonoBehaviour
    {
        /// <summary>
        /// String to set on the TextMeshProText.
        /// </summary>
        public LocalizableString Text;

        /// <summary>
        /// Reference to the text to set.
        /// </summary>
        private TMP_Text textMeshProText;

        /// <summary>
        /// Set the text.
        /// </summary>
        private void OnEnable()
        {
            textMeshProText = GetComponent<TMP_Text>();
            SetText();
            LocalizationManager.Instance.LanguageChanged += newLanguage => SetText();
        }

        /// <summary>
        /// Set the text.
        /// </summary>
        private void SetText() => textMeshProText.SetText(Text.LocalizedValue);
    }
}