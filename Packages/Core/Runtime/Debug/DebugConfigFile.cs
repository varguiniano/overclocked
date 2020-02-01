using System;
using UnityEngine;
using Varguiniano.Core.Runtime.Configuration;

namespace Varguiniano.Core.Runtime.Debug
{
    /// <summary>
    /// Class representing the debug configuration file.
    /// </summary>
    [CreateAssetMenu(menuName = "Varguiniano/Configuration/Debug", fileName = "DebugConfig")]
    public class DebugConfigFile : ConfigFile<DebugConfig>
    {
    }

    /// <summary>
    /// Configuration data for debugging.
    /// </summary>
    [Serializable]
    public class DebugConfig : ConfigData
    {
        /// <summary>
        /// Log level to log.
        /// </summary>
        public LogType LogLevel;
        
        /// <summary>
        /// Should the console ve accessible in game?
        /// </summary>
        public bool InGameConsole;

        /// <summary>
        /// Maximum number of logs stored at runtime.
        /// </summary>
        public int MaxLogsStoredAtRuntime = 5000;

        /// <summary>
        /// 0 is infinite.
        /// </summary>
        public int MaxNumberOfLogFiles = 5;
    }
}