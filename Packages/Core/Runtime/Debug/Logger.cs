using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;
using Varguiniano.Core.Runtime.Configuration;
using Object = UnityEngine.Object;

#if UNITY_WEBGL && !UNITY_EDITOR
using Varguiniano.Core.Runtime.WebGL;
#endif

namespace Varguiniano.Core.Runtime.Debug
{
    /// <summary>
    /// Wrapper class for Debug.Log that makes sure the logs are easy readable with ConsolePro.
    /// It also saves the logs to runtime memory to be accessible by other classes and to a log file.
    /// </summary>
    public static class Logger
    {
        #region Log Methods

        /// <summary>
        /// Log an info message.
        /// </summary>
        /// <param name="message">The message to log.</param>
        /// <param name="sender">The sender, this will probable always be "this".</param>
        public static void LogInfo(string message, Object sender) => Log(message, LogType.Info, sender);

        /// <summary>
        /// Log an warning message.
        /// </summary>
        /// <param name="message">The message to log.</param>
        /// <param name="sender">The sender, this will probable always be "this".</param>
        public static void LogWarning(string message, Object sender) => Log(message, LogType.Warning, sender);

        /// <summary>
        /// Log an error message.
        /// </summary>
        /// <param name="message">The message to log.</param>
        /// <param name="sender">The sender, this will probable always be "this".</param>
        public static void LogError(string message, Object sender) => Log(message, LogType.Error, sender);

        /// <summary>
        /// Log an info message.
        /// This method is useful for static classes that can't send "this" as a sender.
        /// </summary>
        /// <param name="message">The message to log.</param>
        /// <param name="sender">The sender, useful for static classes that can't send "this".</param>
        public static void LogInfo(string message, string sender) => Log(message, LogType.Info, sender);

        /// <summary>
        /// Log an warning message.
        /// This method is useful for static classes that can't send "this" as a sender.
        /// </summary>
        /// <param name="message">The message to log.</param>
        /// <param name="sender">The sender, useful for static classes that can't send "this".</param>
        public static void LogWarning(string message, string sender) => Log(message, LogType.Warning, sender);

        /// <summary>
        /// Log an error message.
        /// This method is useful for static classes that can't send "this" as a sender.
        /// </summary>
        /// <param name="message">The message to log.</param>
        /// <param name="sender">The sender, useful for static classes that can't send "this".</param>
        public static void LogError(string message, string sender) => Log(message, LogType.Error, sender);

        /// <summary>
        /// Log a message.
        /// </summary>
        /// <param name="message">The message to log.</param>
        /// <param name="logType">Log type.</param>
        /// <param name="sender">The sender, this will probable always be "this".</param>
        private static void Log(string message, LogType logType, Object sender) =>
            Log(message, logType, sender.GetType().Name, sender);

        /// <summary>
        /// Log a message.
        /// This method is useful for static classes that can't send "this" as a sender.
        /// </summary>
        /// <param name="message">The message to log.</param>
        /// <param name="logType">Log type.</param>
        /// <param name="sender">The sender as a string, useful for static classes that can't send "this".</param>
        private static void Log(string message, LogType logType, string sender) => Log(message, logType, sender, null);

        /// <summary>
        /// Log a message.
        /// Check first for config and enqueue the message if necessary.
        /// </summary>
        /// <param name="message">The message to log.</param>
        /// <param name="logType">Log type.</param>
        /// <param name="sender">The sender as a string.</param>
        /// <param name="context">The context of the message, this will normally be the sender or this class.</param>
        private static void Log(string message, LogType logType, string sender, Object context)
        {
            // Log everything if application is not playing.
            if (Application.isPlaying)
            {
                if (config == null)
                {
                    LogQueue.Enqueue(new QueueableLog(message, logType, sender, context));

                    if (askedForInitialization) return;
                    askedForInitialization = true;

                    ConfigurationManager.OnServiceReady += service =>
                                                           {
                                                               config = service.GetConfig<DebugConfig>();

                                                               if (config == null)
                                                                   UnityEngine
                                                                      .Debug
                                                                      .LogError("There is no debug config file. Won't be able to log anything!");
                                                           };

                    return;
                }

                while (LogQueue.Count > 0)
                {
                    QueueableLog log = LogQueue.Dequeue();
                    LogSingleMessage(log.Message, log.LogType, log.Sender, log.Context);
                }
            }

            LogSingleMessage(message, logType, sender, context);
        }

        /// <summary>
        /// Log a message.
        /// </summary>
        /// <param name="message">The message to log.</param>
        /// <param name="logType">Log type.</param>
        /// <param name="sender">The sender as a string.</param>
        /// <param name="context">The context of the message, this will normally be the sender or this class.</param>
        private static void LogSingleMessage(string message, LogType logType, string sender, Object context)
        {
            // Remove the spaces from the sender and add a space at the end. That's how ConsolePro likes it.
            string log = "#" + sender.Replace(" ", string.Empty) + " #" + message;

            if (!Application.isPlaying || logType >= config.LogLevel)
            {
                switch (logType)
                {
                    case LogType.Info:
                        UnityEngine.Debug.Log(log, context);
                        break;
                    case LogType.Warning:
                        UnityEngine.Debug.LogWarning(log, context);
                        break;
                    case LogType.Error:
                        UnityEngine.Debug.LogError(log, context);
                        break;
                }

                LogMessageToRuntimeMemory(logType, sender, message);
            }

            // We log to file no matter the log level, just in case.
            SaveLogToFile(message, logType, sender);
        }

        /// <summary>
        /// Logs an exception.
        /// </summary>
        /// <param name="exception">The exception to log.</param>
        /// <param name="sender">The sender, this will probable always be "this".</param>
        public static void LogException(Exception exception, Object sender) =>
            UnityEngine.Debug.LogException(exception, sender);

        /// <summary>
        /// Logs an exception.
        /// </summary>
        /// <param name="exception">The exception to log.</param>
        public static void LogException(Exception exception) => UnityEngine.Debug.LogException(exception);

        /// <summary>
        /// Don't use this method to log exceptions, this should only be used by the Exception catcher.
        /// </summary>
        /// <param name="message">Exception message.</param>
        /// <param name="stacktrace">Exception stacktrace.</param>
        public static void LogException(string message, string stacktrace)
        {
            LogMessageToRuntimeMemory(LogType.Exception, "Exception system", message);
            SaveExceptionToFile(message, stacktrace);
        }

        #endregion

        #region Runtime Log Storage

        /// <summary>
        /// List that stores the log messages for this session.
        /// </summary>
        public static readonly List<LogMessage> LogMessages = new List<LogMessage>();

        /// <summary>
        /// Saves a message to the list.
        /// </summary>
        /// <param name="logType">Log type.</param>
        /// <param name="sender">Sender of the message.</param>
        /// <param name="message">Message.</param>
        private static void LogMessageToRuntimeMemory(LogType logType, string sender, string message)
        {
            if (!Application.isPlaying) return;
            LogMessages.Add(new LogMessage(DateTime.Now.ToString("yyyy-MM-dd-hh-mm-ss-fff"), logType, sender, message));
            if (LogMessages.Count > config.MaxLogsStoredAtRuntime) LogMessages.RemoveAt(0);
        }

        #endregion

        #region Log Persistence

        /// <summary>
        /// File name for the log file of this session.
        /// </summary>
        private static string FileName
        {
            get
            {
                if (string.IsNullOrEmpty(fileName)) fileName = DateTime.Now.ToString("yyyy-MM-dd-hh-mm-ss") + ".log";
                return fileName;
            }
        }

        /// <summary>
        /// Backfield for FileName.
        /// </summary>
        private static string fileName;

        /// <summary>
        /// Path to the logs folder.
        /// </summary>
        private static string LogsPath
        {
            get
            {
                if (string.IsNullOrEmpty(logsPath)) logsPath = Application.persistentDataPath + "\\Logs";
                return logsPath;
            }
        }

        /// <summary>
        /// Backfield for LogsPath.
        /// </summary>
        private static string logsPath;

        /// <summary>
        /// File path for this session log.
        /// </summary>
        public static string FilePath
        {
            get
            {
                if (string.IsNullOrEmpty(filePath)) filePath = LogsPath + "\\" + FileName;
                return filePath;
            }
        }

        /// <summary>
        /// Backfield for FilePath;
        /// </summary>
        private static string filePath;

        /// <summary>
        /// Bool to know if there are existing logs files.
        /// </summary>
        private static bool LogFileExists => Directory.Exists(LogsPath) && Directory.GetFiles(LogsPath).Length > 0;

        /// <summary>
        /// Flag to know if we already deleted files once this session.
        /// </summary>
        private static bool deletedFilesOnceThisSession;

        /// <summary>
        /// Saves a common log to file.
        /// </summary>
        /// <param name="message">The message to log.</param>
        /// <param name="logType">Log type.</param>
        /// <param name="sender">The sender as a string.</param>
        private static void SaveLogToFile(string message, LogType logType, string sender)
        {
            StringBuilder msg = new StringBuilder();
            msg.Append("[");
            msg.Append(DateTime.Now.ToString("yyyy-MM-dd-hh-mm-ss-fff"));
            msg.Append("] [");
            msg.Append(logType);
            msg.Append("] ");
            msg.Append(sender);
            msg.Append(": ");
            msg.Append(message);
            SaveLineToFile(msg.ToString());
        }

        /// <summary>
        /// Saves an exception to file.
        /// </summary>
        /// <param name="message">Exception message.</param>
        /// <param name="stacktrace">Stacktrace.</param>
        private static void SaveExceptionToFile(string message, string stacktrace)
        {
            StringBuilder msg = new StringBuilder();
            msg.Append("[");
            msg.Append(DateTime.Now.ToString("yyyy-MM-dd-hh-mm-ss-fff"));
            msg.Append("] [Exception] ");
            msg.Append(message);
            msg.Append("\nStacktrace: ");
            msg.Append(stacktrace);
            SaveLineToFile(msg.ToString());
        }

        /// <summary>
        /// Saves a log line to file.
        /// </summary>
        /// <param name="line">The line to save.</param>
        private static void SaveLineToFile(string line)
        {
            // Prevent saving editor logs.
            if (!Application.isPlaying) return;

            if (!Directory.Exists(LogsPath)) Directory.CreateDirectory(LogsPath);

            using (StreamWriter writer = File.AppendText(FilePath)) writer.WriteLine(line);

            #if UNITY_WEBGL && !UNITY_EDITOR
            WebGlUtils.SyncFiles();
            #endif

            if (!deletedFilesOnceThisSession) CleanUpOldFiles();
        }

        /// <summary>
        /// Delete old files files if the config says so.
        /// </summary>
        private static void CleanUpOldFiles()
        {
            if (!LogFileExists) return;
            if (config.MaxNumberOfLogFiles == 0) return; // 0 = infinity

            List<string> files = Directory.GetFiles(LogsPath).OrderBy(x => x).ToList();

            if (files.Count < config.MaxNumberOfLogFiles) return;

            int filesToDelete = files.Count - config.MaxNumberOfLogFiles;

            for (int i = 0; i < filesToDelete; ++i) File.Delete(files[i]);

            #if UNITY_WEBGL && !UNITY_EDITOR
            WebGlUtils.SyncFiles();
            #endif

            deletedFilesOnceThisSession = true;
        }

        #endregion

        #region Log Queue

        /// <summary>
        /// Queue of logs that are stored until we are ready to log them.
        /// </summary>
        private static readonly Queue<QueueableLog> LogQueue = new Queue<QueueableLog>();

        #endregion

        #region Configuration

        /// <summary>
        /// Reference to the debug configuration.
        /// </summary>
        private static DebugConfig config;

        /// <summary>
        /// Did we ask for manager initialization?
        /// </summary>
        private static bool askedForInitialization;

        #endregion
    }

    /// <summary>
    /// Class that represents a log message.
    /// </summary>
    [Serializable]
    public struct LogMessage
    {
        /// <summary>
        /// When the message was logged.
        /// </summary>
        public string Time;

        /// <summary>
        /// Log type.
        /// </summary>
        public LogType LogType;

        /// <summary>
        /// Log sender.
        /// </summary>
        public string Sender;

        /// <summary>
        /// Log message.
        /// </summary>
        public string Message;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="time">When the message was logged.</param>
        /// <param name="logType">Log type.</param>
        /// <param name="sender">Log sender.</param>
        /// <param name="message">Log message.</param>
        public LogMessage(string time, LogType logType, string sender, string message)
        {
            Time = time;
            LogType = logType;
            Sender = sender;
            Message = message;
        }
    }

    /// <summary>
    /// Class to store the logs until we are ready to log them.
    /// </summary>
    [Serializable]
    public struct QueueableLog
    {
        /// <summary>
        /// Message to log.
        /// </summary>
        public string Message;

        /// <summary>
        /// Type of log.
        /// </summary>
        public LogType LogType;

        /// <summary>
        /// Log sender.
        /// </summary>
        public string Sender;

        /// <summary>
        /// Context of the log.
        /// </summary>
        public Object Context;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="message">Message to log.</param>
        /// <param name="logType">Type of log.</param>
        /// <param name="sender">Log sender.</param>
        /// <param name="context">Context of the log.</param>
        public QueueableLog(string message, LogType logType, string sender, Object context)
        {
            Message = message;
            LogType = logType;
            Sender = sender;
            Context = context;
        }
    }

    /// <summary>
    /// Enum for different log types.
    /// </summary>
    public enum LogType
    {
        Info,
        Warning,
        Error,
        Exception
    }
}