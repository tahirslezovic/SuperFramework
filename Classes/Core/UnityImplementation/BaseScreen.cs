using SuperFramework.Interfaces;
using System;
using System.Threading.Tasks;
using UnityEngine;
using ILogger = SuperFramework.Interfaces.ILogger;

namespace SuperFramework.Classes.Core
{
    public abstract class BaseScreen : MonoBehaviour, IView
    {
        /// <summary>
        /// Screen name, it should be unique.
        /// </summary>
        public abstract string Name { get; }

        /// <summary>
        /// True if screen is visible, false otherwise.
        /// </summary>
        public bool IsVisible { get; protected set; }

        /// <summary>
        /// True if screen is initialized, false otherwise.
        /// </summary>
        public bool IsInitialized { get; protected set; }

        /// <summary>
        /// Unique screen id.
        /// </summary>
        public abstract int Id { get; }

        protected ILogger _logger;
        protected CanvasGroup _canvasGroup;

        /// <summary>
        /// Hide screen
        /// </summary>
        /// <param name="onHideStart">Method called when hide animation has started</param>
        /// <param name="onHideCompleted">Method called when hide animation has completeds</param>
        public virtual void Hide(Action onHideStart = null, Action onHideCompleted = null)
        {
            onHideStart?.Invoke();
            onHideCompleted?.Invoke();
        }

        public virtual void InitializationFailed(ILogger logger, Exception e)
        {
            // Nothing
        }

        /// <summary>
        /// Async screen initialization
        /// </summary>
        /// <param name="logger">Logger</param>
        /// <returns>Task object</returns>
        public virtual Task InitializeAsync(ILogger logger = null)
        {
            _logger = logger;
            _canvasGroup = GetComponent<CanvasGroup>();
            IsInitialized = true;
            return Task.CompletedTask;
        }

        /// <summary>
        /// Refresh screen
        /// </summary>
        /// <param name="data">Data to refresh</param>
        /// <param name="onRefreshCompleted">Method called when refresh has been completed</param>
        public virtual void Refresh(IViewData data = null, Action onRefreshCompleted = null)
        {
            onRefreshCompleted?.Invoke();
        }

        /// <summary>
        /// Show screen
        /// </summary>
        /// <param name="data">Data to display on show</param>
        /// <param name="onShowStart">Method called when show animation has started</param>
        /// <param name="onShowCompleted">Method called when show animation has completed</param>
        public virtual void Show(IViewData data = null, Action onShowStart = null, Action onShowCompleted = null)
        {
            onShowStart?.Invoke();
            onShowCompleted?.Invoke();
        }
    }
}
