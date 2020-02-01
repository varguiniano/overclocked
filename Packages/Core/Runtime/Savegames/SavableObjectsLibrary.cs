using System.Collections.Generic;
using UnityEngine;

namespace Varguiniano.Core.Runtime.Savegames
{
    /// <summary>
    /// Library that stores all the objects that want to save data.
    /// </summary>
    [CreateAssetMenu(menuName = "Varguiniano/Savegames/SavableObjectsLibrary", fileName = "SavableObjectsLibrary")]
    public class SavableObjectsLibrary : ScriptableObject
    {
        /// <summary>
        /// List of savable objects.
        /// </summary>
        public List<SavableObject> SavableObjects = new List<SavableObject>();
    }
}