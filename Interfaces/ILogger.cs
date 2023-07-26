using System;

namespace SuperFramework.Interfaces
{
    public interface ILogger
    {
        void Log(object message, string tag = null);
        void LogError(object message, string tag = null);
        void LogWarning(object message, string tag = null);
        void LogException(object message, Exception exception, string tag = null);
    }
}
