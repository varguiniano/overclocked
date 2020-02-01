using System;
using System.Collections;
using UnityEngine;
using Varguiniano.Core.Runtime.Common;
using Varguiniano.Core.Runtime.Configuration;
using Logger = Varguiniano.Core.Runtime.Debug.Logger;

namespace Varguiniano.Core.Runtime.Screen
{
    /// <summary>
    /// Class that handles screen resolution.
    /// </summary>
    public class ScreenManager : Singleton<ScreenManager>
    {
        /// <summary>
        /// Event raised when the resolution is updated.
        /// </summary>
        public Action<int, int> ResolutionUpdated;

        /// <summary>
        /// Reference to the configuration manager.
        /// </summary>
        private ConfigurationManager configManager;

        /// <summary>
        /// Flag to know when we are updating the config file.
        /// </summary>
        private bool fileUpdating;

        /// <summary>
        /// Wait for a second.
        /// </summary>
        private readonly WaitForSeconds waitForASecond = new WaitForSeconds(1);

        /// <summary>
        /// Subscribe to the configuration manager.
        /// </summary>
        private void OnEnable() => ConfigurationManager.OnServiceReady += ConfigManagerReady;

        /// <summary>
        /// Unsubscribe from the configuration manager.
        /// </summary>
        private void OnDisable() => ConfigurationManager.OnServiceReady -= ConfigManagerReady;

        /// <summary>
        /// Called when the configuration manager is ready.
        /// </summary>
        /// <param name="manager">The configuration manager instance.</param>
        private void ConfigManagerReady(ConfigurationManager manager) => configManager = manager;

        /// <summary>
        /// Updates the screen resolution from new parameters.
        /// Also saves these parameters to the config file.
        /// Keep in mind that the full screen mode can override the resolution.
        /// </summary>
        /// <param name="width">New width.</param>
        /// <param name="height">New height.</param>
        /// <param name="fullScreenMode">New mode.</param>
        public void UpdateScreenResolution(int width, int height, FullScreenMode fullScreenMode)
        {
            StartCoroutine(UpdateScreenResolutionFileRoutine(width, height, fullScreenMode));
            UpdateScreenResolution();
        }

        /// <summary>
        /// Updates the screen resolution from the config file.
        /// </summary>
        public void UpdateScreenResolution() => StartCoroutine(UpdateScreenResolutionRoutine());

        /// <summary>
        /// Routine to update the screen resolution from the config file.
        /// </summary>
        private IEnumerator UpdateScreenResolutionRoutine()
        {
            #if UNITY_EDITOR || UNITY_WEBGL
            Logger.LogWarning("Running on a platform unaffected by the resolution changer, not updating the resolution.",
                              this);

            yield break;
            #endif

            #pragma warning disable 162
            // ReSharper disable HeuristicUnreachableCode
            #pragma warning disable 162
            while (configManager == null)
            {
                Logger.LogInfo("Waiting for config manager initialization.", this);
                yield return waitForASecond;
            }

            while (fileUpdating) yield return waitForASecond;

            ScreenConfiguration config = configManager.GetConfig<ScreenConfiguration>();
            UnityEngine.Screen.SetResolution(config.Width, config.Height, config.FullScreen);

            Logger.LogInfo("Updated screen resolution to "
                         + config.Width
                         + "x"
                         + config.Height
                         + " on "
                         + config.FullScreen
                         + " mode.",
                           this);
            #pragma warning restore 162

            ResolutionUpdated?.Invoke(UnityEngine.Screen.width, UnityEngine.Screen.height);
        }

        /// <summary>
        /// Routine to update the screen resolution file.
        /// </summary>
        /// <param name="width">New width.</param>
        /// <param name="height">New height.</param>
        /// <param name="fullScreenMode">New mode.</param>
        private IEnumerator UpdateScreenResolutionFileRoutine(int width, int height, FullScreenMode fullScreenMode)
        {
            fileUpdating = true;

            while (configManager == null)
            {
                Logger.LogInfo("Waiting for config manager initialization.", this);
                yield return waitForASecond;
            }

            ScreenConfiguration configData = new ScreenConfiguration
                                             {
                                                 Width = width,
                                                 Height = height,
                                                 FullScreen = fullScreenMode
                                             };

            configManager.SetConfig(configData);

            fileUpdating = false;
        }

        /// <summary>
        /// Called when the application changes focus.
        /// </summary>
        /// <param name="hasFocus">If it has focus.</param>
        private void OnApplicationFocus(bool hasFocus) =>
            ResolutionUpdated?.Invoke(UnityEngine.Screen.width, UnityEngine.Screen.height);
    }
}