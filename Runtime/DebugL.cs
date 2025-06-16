using UnityEngine;
using System;
using System.IO;
using System.Text;
using System.Collections.Generic;

namespace DevLeeJ05.DebugL {
    public static class DebugL
    {
        private static string tag = "[DebugL] ";
        public static string LogFilePath { get; set; } = null;
        public static string LogFileName { get; set; } = $"{tag} Log.txt";
        public static LogType LogTypesToSave { get; set; } = LogType.Error | LogType.Exception;
        public static bool EnableUnityConsoleOutput { get; set; } = true;
    
        private static List<LogEntry> logEntries = new List<LogEntry>();
        private static string fullLogFilePath;
        private static bool isInitialized = false;
        
        private static Action<string, string, LogType> customLogAction;
    
        public static void Initialize()
        {
            if (isInitialized)
            {
                return;
            }
    
            fullLogFilePath = Path.Combine(LogFilePath == null ? Application.persistentDataPath : LogFilePath, LogFileName);
    
            Application.logMessageReceived += HandleLog;
            Application.quitting += OnApplicationQuitting;
    
            customLogAction = (logString, stackTrace, type) =>
            {
                if ((LogTypesToSave & type) != 0)
                {
                    LogEntry newLog = new LogEntry
                    {
                        timestamp = DateTime.Now,
                        type = type,
                        message = logString,
                        stackTrace = stackTrace
                    };
                    logEntries.Add(newLog);
                }
    
                if (EnableUnityConsoleOutput)
                {
                    if (type == LogType.Exception)
                    {
                        Debug.unityLogger.LogError(tag, $"{logString}\n{stackTrace}");
                    }
                    else
                    {
                        Debug.unityLogger.Log(type, tag, logString);
                    }
                }
            };
    
            isInitialized = true;
            Debug.unityLogger.Log(tag, "Custom log handler set up and initialized.");
        }
    
        public static void Dispose()
        {
            if (!isInitialized)
            {
                return;
            }
    
            Application.logMessageReceived -= HandleLog;
            Application.quitting -= OnApplicationQuitting;
            SaveLogsToFile();
            isInitialized = false;
            Debug.unityLogger.Log(tag, "Log handler disposed and logs saved.");
        }
    
        private static void OnApplicationQuitting()
        {
            Dispose();
        }
    
        private static void HandleLog(string logString, string stackTrace, LogType type)
        {
            if ((LogTypesToSave & type) != 0)
            {
                LogEntry newLog = new LogEntry
                {
                    timestamp = DateTime.Now,
                    type = type,
                    message = logString,
                    stackTrace = stackTrace
                };
                logEntries.Add(newLog);
            }
        }
    
        public static void Log(object message)
        {
            if (!isInitialized) Initialize();
            customLogAction?.Invoke(message?.ToString(), StackTraceUtility.ExtractStackTrace(), LogType.Log);
        }
    
        public static void LogWarning(object message)
        {
            if (!isInitialized) Initialize();
            customLogAction?.Invoke(message?.ToString(), StackTraceUtility.ExtractStackTrace(), LogType.Warning);
        }
    
        public static void LogError(object message)
        {
            if (!isInitialized) Initialize();
            customLogAction?.Invoke(message?.ToString(), StackTraceUtility.ExtractStackTrace(), LogType.Error);
        }
    
        public static void LogException(Exception exception)
        {
            if (!isInitialized) Initialize();
            customLogAction?.Invoke(exception?.Message, exception?.StackTrace, LogType.Exception);
        }
    
        public static void SaveLogsToFile()
        {
            StringBuilder sb = new StringBuilder();
            foreach (var entry in logEntries)
            {
                sb.AppendLine($"[{entry.timestamp:yyyy-MM-dd HH:mm:ss}] [{entry.type}] {entry.message}");
                if (!string.IsNullOrEmpty(entry.stackTrace))
                {
                    sb.AppendLine(entry.stackTrace);
                }
                sb.AppendLine("---");
            }
    
            try
            {
                File.WriteAllText(fullLogFilePath, sb.ToString());
                Debug.unityLogger.Log(tag, "Logs saved to: " + fullLogFilePath);
            }
            catch (Exception e)
            {
                Debug.unityLogger.LogError(tag, "Failed to save logs: " + e.Message);
            }
        }
    
        public static void ClearLogs()
        {
            logEntries.Clear();
            Debug.unityLogger.Log(tag, "Logs cleared.");
        }
    }
    
    [System.Serializable]
    public class LogEntry
    {
        public DateTime timestamp;
        public LogType type;
        public string message;
        public string stackTrace;
    }
}
