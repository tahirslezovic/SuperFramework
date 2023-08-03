using SuperFramework.Interfaces;
using System;
using System.Threading.Tasks;
using UnityEngine;
using ILogger = SuperFramework.Interfaces.ILogger;

namespace SuperFramework.Classes.Core
{
    /// <summary>
    /// Base pop-up implementation.
    /// </summary>
    public abstract class BasePopUp : MonoBehaviour, IPopUp
    {
        /// <summary>
        /// Overlay name, it should be unique.
        /// </summary>
        public abstract string Name { get; }

        /// <summary>
        /// True if overlay is visible, false otherwise.
        /// </summary>
        public bool IsVisible { get; protected set; }

        /// <summary>
        /// True if overlay is initialized, false otherwise.
        /// </summary>
        public bool IsInitialized { get; protected set; }

        /// <summary>
        /// Unique overlay id.
        /// </summary>
        public abstract int Id { get; }

        /// <summary>
        /// Logger
        /// </summary>
        protected ILogger _logger;

        /// <summary>
        /// Canvas group component (It can be null if component is not attached to this game object)
        /// </summary>
        protected CanvasGroup _canvasGroup;

        /// <summary>
        /// Hide pop-up
        /// </summary>
        /// <param name="onHideStart">Method called when hide animation has started</param>
        /// <param name="onHideCompleted">Method called when hide animation has completeds</param>
        public virtual void Hide(Action onHideStart = null, Action onHideCompleted = null)
        {
            IsVisible = false;
            if (_canvasGroup != null)
            {
                _canvasGroup.alpha = 0;
                _canvasGroup.blocksRaycasts = false;
            }

            onHideStart?.Invoke();
            onHideCompleted?.Invoke();
        }

        public virtual void InitializationFailed(ILogger logger, Exception e)
        {
            // Nothing
        }

        /// <summary>
        /// Async initialize pop-up
        /// </summary>
        /// <param name="logger">Logger</param>
        /// <returns>Task object</returns>
        public virtual Task InitializeAsync(ILogger logger = null)
        {
            _canvasGroup = GetComponent<CanvasGroup>();

            IsInitialized = true;
            _logger = logger;
            return Task.CompletedTask;
        }

        /// <summary>
        /// Refresh pop-up
        /// </summary>
        /// <param name="data">Data to refresh</param>
        /// <param name="onRefreshCompleted">Method called when refresh has been completed</param>
        public virtual void Refresh(IViewData data = null, Action onRefreshCompleted = null)
        {
            onRefreshCompleted?.Invoke();
        }

        /// <summary>
        /// Show pop-up
        /// </summary>
        /// <param name="data">Data to display on show</param>
        /// <param name="onShowStart">Method called when show animation has started</param>
        /// <param name="onShowCompleted">Method called when show animation has completed</param>
        public virtual void Show(IViewData data = null, Action onShowStart = null, Action onShowCompleted = null)
        {
            IsVisible = true;
            if (_canvasGroup != null)
            {
                _canvasGroup.alpha = 1;
                _canvasGroup.blocksRaycasts = true;
            }

            onShowStart?.Invoke();
            onShowCompleted?.Invoke();
        }
    }
}
