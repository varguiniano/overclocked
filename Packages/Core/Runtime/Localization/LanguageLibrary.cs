using System.Collections.Generic;
using UnityEngine;

namespace Varguiniano.Core.Runtime.Localization
{
    /// <summary>
    /// Library that holds all the available language assets.
    /// </summary>
    [CreateAssetMenu(menuName = "Varguiniano/Localization/LanguageLibrary", fileName = "LanguageLibrary")]
    public class LanguageLibrary : ScriptableObject
    {
        /// <summary>
        /// List of languages.
        /// </summary>
        public List<Language> Languages = new List<Language>();

        /// <summary>
        /// Retrieves a list of the language names.
        /// </summary>
        public List<LocalizableString> LanguageNames
        {
            get
            {
                List<LocalizableString> nameList = new List<LocalizableString>();

                for (int i = 0; i < Languages.Count; ++i)
                    nameList.Add(new LocalizableString
                                 {
                                     Key =
                                         Languages[i].Key
                                 });

                return nameList;
            }
        }
    }
}