using UnityEngine;

namespace Varguiniano.Core.Runtime.Screen
{
    /// <summary>
    /// Class that sets the screen configuration on awake.
    /// Useful to place on the first scene, so the game starts with the resolution set on the config file.
    /// </summary>
    public class ScreenResolutionInitializer : MonoBehaviour
    {
        /// <summary>
        /// Tell the screen manager to update the screen resolution and autodestroy.
        /// </summary>
        private void Awake()
        {
            ScreenManager.Instance.UpdateScreenResolution();
            Destroy(gameObject);
        }
    }
}