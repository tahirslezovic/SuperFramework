﻿using SuperFramework.Interfaces;
using System;
using System.Threading.Tasks;
using UnityEngine;
using ILogger = SuperFramework.Interfaces.ILogger;

namespace SuperFramework.Classes.Core
{
    /// <summary>
    // Base overlay implementation
    /// </summary>
    public abstract class BaseOverlay : MonoBehaviour, IOverlay
    {

        #region Fields
        /// <summary>
        /// Logger
        /// </summary>
        protected ILogger _logger;


        /// <summary>
        /// Canvas group component (It can be null if component is not attached to this game object)
        /// </summary>
        protected CanvasGroup _canvasGroup;

        /// <summary>
        /// Animator component (It can be null if component is not attached game object)
        /// </summary>
        protected Animator _animator;
        #endregion

        #region Properties

        public abstract string Name { get; }

        public bool IsVisible { get; protected set; }

        public bool IsInitialized { get; protected set; }

        public abstract int Id { get; }

        #endregion


        #region API

        public virtual void InitializationFailed(ILogger logger, Exception e) => logger?.LogException("Initialization failed!", e, Name);

        public virtual Task InitializeAsync(ILogger logger = null)
        {
            _canvasGroup = GetComponent<CanvasGroup>();
            _animator = GetComponent<Animator>();

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
            IsVisible = true;
            if (_canvasGroup != null)
            {
                _canvasGroup.alpha = 1;
                _canvasGroup.blocksRaycasts = true;
            }

            onShowStart?.Invoke();
            onShowCompleted?.Invoke();
        }

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

        #endregion

    }
}
