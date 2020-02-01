using System;

namespace Varguiniano.Core.Runtime.Localization
{
    /// <summary>
    /// Struct that represents a localizable string.
    /// </summary>
    [Serializable]
    public class LocalizableString
    {
        /// <summary>
        /// Key to search in the localization manager.
        /// </summary>
        public string Key;

        /// <summary>
        /// Localized value of this string.
        /// </summary>
        public string LocalizedValue => LocalizationManager.Instance[Key];
    }
}