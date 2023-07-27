using System;

namespace SuperFramework.Interfaces
{
    /// <summary>
    /// Base interface for each logger which exists in the game, e.g UnityLogger, ProductionLogger, etc..
    /// </summary>
    public interface ILogger
    {
        void Log(object message, string tag = null);
        void LogError(object message, string tag = null);
        void LogWarning(object message, string tag = null);
        void LogException(object message, Exception exception, string tag = null);
    }
}
