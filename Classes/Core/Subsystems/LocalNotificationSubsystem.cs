using SuperFramework.Interfaces;
using System;
using System.Threading.Tasks;

namespace SuperFramework.Core
{
    public interface ILocalNotification
    {

    };

    /// <summary>
    /// System that handles local notification scheduling.
    /// You should extend this class and write concrete implementation in the Unity.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class LocalNotificationSubsystem<T> : ISubsystem where T : ILocalNotification
    {
        public abstract string Name { get; }

        public bool IsInitialized { get; protected set; }

        protected readonly ILogger _logger;
        public LocalNotificationSubsystem(ILogger logger)
        {
            _logger = logger;
        }

        public virtual void InitializationFailed(ILogger logger, Exception e)
        {

        }

        public virtual Task InitializeAsync(ILogger logger = null)
        {
            return Task.CompletedTask;
        }

        public virtual Task PauseSystemAsync()
        {
            return Task.CompletedTask;
        }

        public virtual Task ResumeSystemAsync()
        {
            return Task.CompletedTask;
        }

        public virtual Task ShutdownAsync()
        {
            return Task.CompletedTask;
        }

        /// <summary>
        /// Schedule a local notification.
        /// </summary>
        /// <param name="notification">Notification object</param>
        public abstract void ScheduleNotification(T notification);

        /// <summary>
        /// Cancel all scheduled notifications.
        /// </summary>
        public abstract void CancelAllNotifications();

        /// <summary>
        /// Cancel specific notification.
        /// </summary>
        /// <param name="notification">Notification object</param>
        public abstract void CancelNotification(T notification);
    }
}
