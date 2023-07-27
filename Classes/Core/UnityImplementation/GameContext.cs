using SuperFramework.Core;
using UnityEngine;
using ILogger = SuperFramework.Interfaces.ILogger;
using SuperFramework.Classes.Null;

namespace SuperFramework.Classes.Core
{
    /// <summary>
    /// Game context is entry point for each game. There is only ONE game context which have ONE MainSystem. 
    /// Main system is responsible for handling various ISubsystems, which are added only before initialization start.
    /// Specific game should extend GameContext and override respective functions.
    /// </summary>
    public abstract class GameContext : MonoBehaviour
    {

        /// <summary>
        /// Main system
        /// </summary>
        protected MainSystem _mainSystem;
        protected static GameContext _instance;

        /// <summary>
        /// Get game context instance
        /// </summary>
        public static GameContext Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType<GameContext>();
                }

                return _instance;

            }
        }

        /// <summary>
        /// Get default logger subsystem
        /// </summary>
        public ILogger Logger { get; protected set; } = new NullLogger();

        /// <summary>
        /// Unity method
        /// </summary>
        public virtual void Awake()
        {
            gameObject.AddComponent<DontDestroyer>();
        }

        /// <summary>
        /// Unity method
        /// </summary>
        public virtual void Start()
        {
            InitializeSystems();
        }

        /// <summary>
        /// Unity method
        /// </summary>
        public virtual void Update()
        {
            if (_mainSystem != null)
            {
                _mainSystem.Update(Time.deltaTime);
            }
        }

        /// <summary>
        /// Unity method
        /// </summary>
        public virtual async void OnApplicationQuit()
        {
            if (_mainSystem != null)
            {
                await _mainSystem.ShutdownAsync();
            }

        }

        /// <summary>
        /// Unity method
        /// </summary>
        public virtual async void OnApplicationPause(bool pauseStatus)
        {
            if (_mainSystem != null)
            {
                if (pauseStatus)
                {
                    await _mainSystem.PauseSystemAsync();
                }
                else
                {
                    await _mainSystem.ResumeSystemAsync();
                }
            }
        }

        /// <summary>
        /// This function should be overriden and populated in which order subsystem should be added and initialized. 
        /// This is async function, so each subsystem should be awaited
        /// </summary>
        protected async virtual void InitializeSystems()
        {

        }

        /// <summary>
        /// Event called when system initialization has been started
        /// </summary>
        protected virtual void OnSystemsInitializationStarted()
        {

        }

        /// <summary>
        /// Event called when system initialization has been completed
        /// </summary>
        protected virtual void OnSystemsInitializationCompleted()
        {

        }

        /// <summary>
        /// Event called on system initialization progress has been changed
        /// </summary>
        /// <param name="systemName">System which has been initialized</param>
        /// <param name="progress">Initialization progress from 0 to 1</param>
        protected virtual void OnSystemsInitializationProgress(string systemName, float progress)
        {

        }

        /// <summary>
        /// Get subsystem by type
        /// </summary>
        /// <typeparam name="T">Subsystem type</typeparam>
        /// <returns>Subsystem if exists, null otherwise</returns>
        public T GetSubsystem<T>()
        {
            return (T)_mainSystem.GetSubsystem<T>();
        }
    }

    /// <summary>
    /// Game context logger extension
    /// </summary>
    public static class GameContextLoggerExtension
    {
        public static ILogger GetLogger(this MonoBehaviour mono)
        {
            return GameContext.Instance.Logger;
        }
    }
}
