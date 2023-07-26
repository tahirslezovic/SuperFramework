using SuperFramework.Interfaces;
using System;

namespace SuperFramework.Classes.Null
{
    /// <summary>
    /// Default empty (null) logger
    /// </summary>
    public class NullLogger : ILogger
    {
        public void Log(object message, string tag = null)
        {
           
        }

        public void LogError(object message, string tag = null)
        {
           
        }

        public void LogException(object message, Exception exception, string tag = null)
        {
          
        }

        public void LogWarning(object message, string tag = null)
        {
           
        }
    }
}
