using System;
using UnityEngine;
using ILogger = SuperFramework.Interfaces.ILogger;


namespace SuperFramework.Classes.Core
{
    public class UnityLogger : ILogger
    {
        public void Log(object message, string tag = null)
        {
            Debug.Log(tag != null ? ($"[{tag}] - " + message) : message);
        }

        public void LogError(object message, string tag = null)
        {
            Debug.LogError(tag != null ? ($"[{tag}] - " + message) : message);
        }

        public void LogException(object message, Exception exception, string tag = null)
        {
            Debug.LogException(exception);
        }

        public void LogWarning(object message, string tag = null)
        {
            Debug.LogWarning(tag != null ? ($"[{tag}] - " + message) : message);
        }
    }
}
