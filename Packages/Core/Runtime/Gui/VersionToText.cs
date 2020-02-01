using TMPro;
using UnityEngine;
using Varguiniano.Core.Runtime.Build;

namespace Varguiniano.Core.Runtime.Gui
{
    /// <summary>
    /// Class that displays the version on a text mesh pro text.
    /// </summary>
    [RequireComponent(typeof(TMP_Text))]
    public class VersionToText : MonoBehaviour
    {
        /// <summary>
        /// Reference to the version.
        /// </summary>
        public Version Version;

        /// <summary>
        /// Set the version text.
        /// </summary>
        private void OnEnable() => GetComponent<TMP_Text>().SetText(Version.FullVersion);
    }
}