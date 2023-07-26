using SuperFramework.Interfaces;
using System;
using System.Threading.Tasks;
using UnityEngine;
using ILogger = SuperFramework.Interfaces.ILogger;

namespace SuperFramework.Classes.Core
{
    public abstract class BaseOverlay : MonoBehaviour, IOverlay
    {
        public abstract string Name { get; }

        public bool IsVisible { get; protected set; }

        public bool IsInitialized { get; protected set; }

        public abstract int Id { get; }

        protected ILogger _logger;
        public virtual void InitializationFailed(ILogger logger, Exception e)
        {

        }

        public virtual Task InitializeAsync(ILogger logger = null)
        {
            IsInitialized = true;
            _logger = logger;
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

        public virtual void Hide(Action onHideStart = null, Action onHideCompleted = null)
        {
            onHideStart?.Invoke();
            onHideCompleted?.Invoke();
        }
    }
}
