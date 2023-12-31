﻿using SuperFramework.Interfaces;
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
        /// Animator component (It can be null if component is not attached game object)
        /// </summary>
        protected Animator _animator;

        /// <summary>
        /// Hide screen
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

        public virtual void InitializationFailed(ILogger logger, Exception e) => logger?.LogException("Initialization failed!", e, Name);

        /// <summary>
        /// Async screen initialization
        /// </summary>
        /// <param name="logger">Logger</param>
        /// <returns>Task object</returns>
        public virtual Task InitializeAsync(ILogger logger = null)
        {
            _logger = logger;
            _canvasGroup = GetComponent<CanvasGroup>();
            _animator = GetComponent<Animator>();   
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
            IsVisible = true;
            if(_canvasGroup != null)
            {
                _canvasGroup.alpha = 1;
                _canvasGroup.blocksRaycasts = true;
            }
            onShowStart?.Invoke();
            onShowCompleted?.Invoke();
        }


        /// <summary>
        /// This function should be called from animation track event when show animation start
        /// </summary>
        public virtual void OnShowAnimationStart()
        {

        }

        /// <summary>
        /// This function should be called from animation track event when show animation has completed
        /// </summary>
        public virtual void OnShowAnimationComplete()
        {

        }


        /// <summary>
        /// This function should be called from animation track event when hide animation start
        /// </summary>
        public virtual void OnHideAnimationStart()
        {

        }

        /// <summary>
        /// This function should be called from animation track event when hide animation complete
        /// </summary>
        public virtual void OnHideAnimationComplete()
        {

        }
    }
}
