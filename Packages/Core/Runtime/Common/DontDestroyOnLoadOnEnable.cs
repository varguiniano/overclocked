using UnityEngine;

namespace Varguiniano.Core.Runtime.Common
{
    /// <summary>
    /// Attach this behaviour to a game object to make it register to don't destroy on load on enable.
    /// </summary>
    public class DontDestroyOnLoadOnEnable : MonoBehaviour
    {
        /// <summary>
        /// Register.
        /// </summary>
        private void OnEnable() => DontDestroyOnLoad(gameObject);
    }
}