using System;
using UnityEngine;
using Varguiniano.Core.Runtime.Common;

namespace Varguiniano.Core.Runtime.Savegames
{
    /// <summary>
    /// Interface used by objects that want some data to be saved when the game is, well, saved.
    /// </summary>
    [Serializable]
    public abstract class SavableObject : ScriptableObject
    {
        /// <summary>
        /// Get the data to save.
        /// </summary>
        /// <returns>The data to save in base64.</returns>
        public string Save() => SaveJson().ToBase64();

        /// <summary>
        /// load the data.
        /// </summary>
        /// <param name="data">Data to load in base64.</param>
        public void Load(string data) => LoadJson(data.FromBase64());

        /// <summary>
        /// Get the data to save.
        /// </summary>
        /// <returns>The data to save.</returns>
        protected abstract string SaveJson();

        /// <summary>
        /// load the data.
        /// </summary>
        /// <param name="data">Data to load.</param>
        protected abstract void LoadJson(string data);
    }
}