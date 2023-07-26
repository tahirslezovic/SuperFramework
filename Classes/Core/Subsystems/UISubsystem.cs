using SuperFramework.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using ILogger = SuperFramework.Interfaces.ILogger;
using ISubsystem = SuperFramework.Interfaces.ISubsystem;

namespace SuperFramework.Classes.Core
{
    public class UISubsystem : MonoBehaviour, ISubsystem
    {
        // Every instantiated screen is attached to this parent.
        protected Transform _screensParent;
        // Every instantiated popUp is attached to this parent.
        protected Transform _popUpsParent;
        // Every instantiated overlay is attached to this parent.
        protected Transform _overlaysParent;

        // screens that can be shown
        protected List<BaseScreen> _allScreens;

        // Active (visible) screen
        protected BaseScreen _currentActiveScreen;
        protected List<BaseOverlay> _activeOverlays;
        protected Dictionary<int, BasePopUp> _activePopUps; // instanceId, object


        public virtual string Name => nameof(UISubsystem);

        public bool IsInitialized { get; protected set; }

        protected ILogger _logger;


        public virtual Task InitializeAsync(ILogger logger = null)
        {
            if (IsInitialized)
                return Task.CompletedTask;

            _logger = logger;

            _screensParent = transform.GetChild(0);
            _overlaysParent = transform.GetChild(1);
            _popUpsParent = transform.GetChild(2);

            _activeOverlays = new List<BaseOverlay>();
            _activePopUps = new Dictionary<int, BasePopUp>();
            _allScreens = new List<BaseScreen>();

            try
            {

                BaseScreen[] screensResources = Resources.LoadAll<BaseScreen>("Views/Screens");

                for (int i = 0; i < screensResources.Length; i++)
                {
                    BaseScreen o = Instantiate(screensResources[i], _screensParent);
                    o.InitializeAsync(logger);
                    _allScreens.Add(o.GetComponent<BaseScreen>());
                }
            }
            catch (Exception e)
            {
                _logger.LogError($"There was a problem while loading all screens {e}");
            }


            IsInitialized = true;

            return Task.CompletedTask;
        }

        public virtual Task InitializeFailedAsync(ILogger logger, Exception e)
        {
            logger.LogError("UISubsystem initialization failed!");
            return Task.CompletedTask;
        }

        protected async Task LoadResource(string path, Action<GameObject> onLoaded)
        {
            try
            {
                ResourceRequest request = Resources.LoadAsync(path, typeof(GameObject));
                while (!request.isDone)
                {
                    await Task.Yield();
                }

                if (request.asset == null)
                {
                    _logger?.LogError("Resource not found, path: " + path);
                }
                else
                {
                    onLoaded?.Invoke(request.asset as GameObject);
                }
            }
            catch (Exception e)
            {
                _logger?.LogError($"There was a problem with resource loading: {e}");
            }
        }

        #region Screen

        /// <summary>
        /// Show specific screen
        /// </summary>
        /// <typeparam name="T">Screen type</typeparam>
        /// <param name="data">Screen data</param>
        /// <param name="screenObject">Screen object</param>
        /// <param name="onShowStarted">On show animation delegate</param>
        /// <param name="onShowCompleted">On hide animation delegate</param>
        public virtual void ShowScreen<T>(IViewData data = null, Action<BaseScreen> screenObject = null, Action onShowStarted = null, Action onShowCompleted = null) where T : BaseScreen
        {
            foreach (var screen in _allScreens)
            {
                if (screen.GetType() == typeof(T))
                {
                    if (_currentActiveScreen == null)
                    {
                        _currentActiveScreen = screen;
                        _currentActiveScreen.Show(data, onShowStarted, onShowCompleted);
                        screenObject?.Invoke(_currentActiveScreen);

                        return;
                    }
                    else
                    {
                        if (_currentActiveScreen.Name == screen.Name)
                        {
                            _logger?.LogWarning($"Screen {_currentActiveScreen.Name} is already visible.");
                            return;
                        }
                        else
                        {
                            _currentActiveScreen.Hide(null, () =>
                            {
                                _currentActiveScreen = screen;
                                _currentActiveScreen.Show(data, onShowStarted, onShowCompleted);

                            });
                            screenObject?.Invoke(screen);
                            return;

                        }
                    }

                }
            }

            _logger?.LogError($"Requested screen type {typeof(T).Name} doesn't exists");
        }

        /// <summary>
        /// Refresh specific screen
        /// </summary>
        /// <typeparam name="T">Screen type</typeparam>
        /// <param name="data">Screen data to refresh</param>
        /// <param name="onRefreshCompleted">On refresh completed delegate</param>
        public virtual void RefreshScreen<T>(IViewData data = null, Action onRefreshCompleted = null)
        {
            foreach (var screen in _allScreens)
            {
                if (screen.GetType() == typeof(T))
                {
                    screen.Refresh(data, onRefreshCompleted);
                    return;
                }
            }

            _logger?.LogError($"Requested screen type {typeof(T).Name} doesn't exists");
        }

        #endregion

        #region PopUp

        /// <summary>
        /// Show specific pop up
        /// </summary>
        /// <typeparam name="T">Pop up type</typeparam>
        /// <param name="data">Data for the pop up</param>
        /// <param name="onInstantiated">Delegate that is called when pop up object is instantiated</param>
        /// <param name="onShowStarted">Delegate when show animation start</param>
        /// <param name="onShowCompleted">Delegate when show animation has completed</param>
        /// <returns>Pop up unique id, or -1 if something is wrong</returns>
        public virtual async Task<int> ShowPopUp<T>(IViewData data = null, Action<IPopUp> onInstantiated = null, Action onShowStarted = null, Action onShowCompleted = null) where T : BasePopUp
        {
            BasePopUp popUp = null;
            try
            {
                await LoadResource("Views/PopUps/" + typeof(T).Name, (popUpAsset) =>
                {
                    GameObject go = Instantiate(popUpAsset, _popUpsParent);
                    popUp = go.GetComponent<BasePopUp>();
                    popUp.InitializeAsync(_logger);
                    onInstantiated?.Invoke(popUp);

                    _activePopUps.Add(popUp.Id, popUp);

                    popUp.Show(data, onShowStarted, onShowCompleted);

                });
            }
            catch (Exception e)
            {
                _logger?.LogError($"Something was wrong while trying to show popUp {typeof(T).Name}, error: {e}");

            }

            return popUp != null ? popUp.Id : -1;
        }

        /// <summary>
        /// Hide (and destroy) specific pop up
        /// </summary>
        /// <param name="instanceId">Pop up unique id</param>
        /// <param name="onHideStarted">Delegate when hide animation start</param>
        /// <param name="onHideCompleted">Delegate when hide animation ends</param>
        public virtual void HidePopUp(int instanceId, Action onHideStarted = null, Action onHideCompleted = null)
        {
            if (_activePopUps.ContainsKey(instanceId))
            {
                _activePopUps[instanceId].Hide(onHideStarted, () =>
                {
                    DestroyImmediate(_activePopUps[instanceId].gameObject);
                    _activePopUps.Remove(instanceId);
                    onHideCompleted?.Invoke();
                });
            }
            else
            {
                _logger?.LogWarning($"Pop up with requested id {instanceId} doesn't exists!");
            }
        }

        /// <summary>
        /// Refresh specific pop up (if pop up is visible and active)
        /// </summary>
        /// <param name="instanceId">Pop up unique id</param>
        /// <param name="data">Data for the pop up</param>
        /// <param name="onRefreshCompleted">Delegate when pop up refreshing has completed</param>
        public virtual void RefreshPopUp(int instanceId, IViewData data = null, Action onRefreshCompleted = null)
        {
            if (_activePopUps.ContainsKey(instanceId))
            {
                _activePopUps[instanceId].Refresh(data, onRefreshCompleted);
            }
            else
            {
                _logger?.LogWarning($"Pop up with requested id {instanceId} doesn't exists!");
            }
        }

        #endregion

        #region Overlay
        /// <summary>
        /// Show specific overlay
        /// </summary>
        /// <typeparam name="T">Overlay type</typeparam>
        /// <param name="data">Data for the overlay</param>
        /// <param name="onInstantiated">Delegate that is called when overlay object is instantiated</param>
        /// <param name="onShowStarted">Delegate when show animation start</param>
        /// <param name="onShowCompleted">Delegate when show animation has completed</param>
        public virtual async Task<int> ShowOverlay<T>(IViewData data = null, Action<IOverlay> onInstantiated = null, Action onShowStarted = null, Action onShowCompleted = null) where T : BaseOverlay
        {
            if (_activeOverlays.Any(o => o.GetType() == typeof(T)))
            {
                _logger?.LogWarning($"Overlay {typeof(T).Name} is already visible.");
                return -1;
            }

            BaseOverlay overlay = null;
            try
            {
                await LoadResource("Views/Overlays/" + typeof(T).Name, (overlayAsset) =>
                {
                    GameObject go = Instantiate(overlayAsset, _overlaysParent);
                    overlay = go.GetComponent<BaseOverlay>();

                    overlay.InitializeAsync(_logger);
                    onInstantiated?.Invoke(overlay);

                    _activeOverlays.Add(overlay);

                    overlay.Show(data, onShowStarted, onShowCompleted);

                });
            }
            catch (Exception e)
            {
                _logger?.LogError($"Something was wrong while trying to show overlay {typeof(T).Name}, error: {e}");
            }

            return overlay == null ? -1 : overlay.Id;
        }

        /// <summary>
        /// Hide (and destroy) specific overlay
        /// </summary>
        /// <param name="onHideStarted">Delegate when hide animation start</param>
        /// <param name="onHideCompleted">Delegate when hide animation end</param>
        public virtual void HideOverlay<T>(Action onHideStarted = null, Action onHideCompleted = null)
        {
            var overlayToHide = _activeOverlays.FirstOrDefault(o => o.GetType() == typeof(T));

            if (overlayToHide != null)
            {
                overlayToHide.Hide(onHideStarted, () =>
                {
                    DestroyImmediate(overlayToHide.gameObject);
                    _activeOverlays.Remove(overlayToHide);
                    onHideCompleted?.Invoke();
                });
            }
            else
            {
                _logger?.LogWarning($"Overlay {typeof(T).Name} doesn't exists!");
            }
        }

        /// <summary>
        /// Refresh specific overlay
        /// </summary>
        /// <param name="data">Data for the overlay</param>
        /// <param name="onRefreshCompleted">Delegate when overlay refreshing has completed</param>
        public virtual void RefreshOverlay<T>(IViewData data = null, Action onRefreshCompleted = null)
        {
            var overlayToRefresh = _activeOverlays.FirstOrDefault(o => o.GetType() == typeof(T));

            if (overlayToRefresh != null)
            {
                overlayToRefresh.Refresh(data, onRefreshCompleted);
            }
            else
            {
                _logger?.LogWarning($"Overlay {typeof(T).Name} doesn't exists!");
            }
        }

        /// <summary>
        /// Get screen by type
        /// </summary>
        /// <typeparam name="T">Screen type</typeparam>
        /// <returns>Screen object if exists, null otherwise</returns>
        public virtual BaseScreen GetScreen<T>()
        {
            var screen = _allScreens.FirstOrDefault(o => o.GetType() == typeof(T));
            if (screen != null)
            {
                return screen;
            }

            _logger.LogWarning($"Requested screen {typeof(T)} type is not found!");

            return null;
        }

        /// Get overlay by type
        /// </summary>
        /// <typeparam name="T">Overlay type</typeparam>
        /// <returns>Overlay object if exists, null otherwise</returns>
        public virtual BaseOverlay GetOverlay<T>()
        {
            var overlay = _activeOverlays.FirstOrDefault(o => o.GetType() == typeof(T));
            if (overlay != null)
            {
                return overlay;
            }

            _logger.LogWarning($"Requested overlay {typeof(T)} type is not found!");

            return null;
        }

        public Task ShutdownAsync()
        {
            return Task.CompletedTask;
        }

        public Task PauseSystemAsync()
        {
            return Task.CompletedTask;
        }

        public Task ResumeSystemAsync()
        {
            return Task.CompletedTask;
        }

        public void InitializationFailed(ILogger logger, Exception e)
        {
           // Nothing
        }


        #endregion

    }
}
