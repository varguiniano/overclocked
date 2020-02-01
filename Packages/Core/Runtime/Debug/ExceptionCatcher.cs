using UnityEngine;
using Varguiniano.Core.Runtime.Common;

namespace Varguiniano.Core.Runtime.Debug
{
    /// <summary>
    /// Class that catches all exceptions the application throws and sends them to the custom logger system.
    /// </summary>
    public class ExceptionCatcher : Singleton<ExceptionCatcher>
    {
        /// <summary>
        /// Subscribe to the log callback.
        /// </summary>
        private void OnEnable()
        {
            Application.logMessageReceived += ExceptionCallBack;
            Logger.LogInfo("Global exception catching ready.", this);
        }

        /// <summary>
        /// Called whenever the unity system logs a message, but we will only use it for exceptions.
        /// </summary>
        /// <param name="condition">Message.</param>
        /// <param name="stacktrace">Stacktrace.</param>
        /// <param name="logType">Log type.</param>
        private static void ExceptionCallBack(string condition, string stacktrace, UnityEngine.LogType logType)
        {
            if (logType == UnityEngine.LogType.Exception || logType == UnityEngine.LogType.Assert)
                Logger.LogException(condition, stacktrace);
        }

        /// <summary>
        /// Unsubscribe from the log callback.
        /// </summary>
        private void OnDisable()
        {
            Application.logMessageReceived -= ExceptionCallBack;
            Logger.LogInfo("Global exception catching off.", this);
        }
    }
}