using SuperFramework.Interfaces;
using System;
using System.Threading.Tasks;
using UnityEngine;
using ILogger = SuperFramework.Interfaces.ILogger;

namespace SuperFramework.Classes.Core
{
    public abstract class BaseScreen : MonoBehaviour, IView
    {
        public abstract string Name { get; }

        public bool IsInitialized { get; protected set; }

        public bool IsVisible { get; protected set; }

        protected ILogger _logger;
        protected CanvasGroup _canvasGroup; 

        public virtual void Hide(Action onHideStart = null, Action onHideCompleted = null)
        {
            onHideStart?.Invoke();
            onHideCompleted?.Invoke();
        }

        public virtual void InitializationFailed(ILogger logger, Exception e)
        {

        }

        public virtual Task InitializeAsync(ILogger logger = null)
        {
            _logger = logger;
            _canvasGroup = GetComponent<CanvasGroup>();
            IsInitialized = true;
            return Task.CompletedTask;
        }

        public virtual void Refresh(IViewData data = null, Action onRefreshCompleted = null)
        {
            onRefreshCompleted?.Invoke();
        }

        public virtual void Show(IViewData data = null, Action onShowStart = null, Action onShowCompleted = null)
        {
            onShowStart?.Invoke();
            onShowCompleted?.Invoke();
        }
    }
}
