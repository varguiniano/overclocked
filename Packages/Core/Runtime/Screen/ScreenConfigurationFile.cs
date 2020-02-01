using System;
using UnityEngine;
using Varguiniano.Core.Runtime.Configuration;

namespace Varguiniano.Core.Runtime.Screen
{
    /// <summary>
    /// Configuration file for the resolution and screen.
    /// </summary>
    [CreateAssetMenu(menuName = "Varguiniano/Configuration/ScreenConfigFile", fileName = "ScreenConfig")]
    public class ScreenConfigurationFile : ConfigFile<ScreenConfiguration>
    {
    }

    /// <summary>
    /// Configuration data for the resolution and screen.
    /// </summary>
    [Serializable]
    public class ScreenConfiguration : ConfigData
    {
        /// <summary>
        /// Width of the window.
        /// </summary>
        public int Width;
        
        /// <summary>
        /// Height of the window.
        /// </summary>
        public int Height;
        
        /// <summary>
        /// Full screen enum.
        /// </summary>
        public FullScreenMode FullScreen;
    }
}