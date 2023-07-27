using SuperFramework.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace SuperFramework.Core
{
    public delegate void OnSystemsInitializationStart();
    public delegate void OnSystemInitializationCompleted();
    public delegate void OnSystemInitializationProgress(string systemName, float progress);

    /// <summary>
    /// Main system only exists in GameContext
    /// </summary>
    public sealed class MainSystem : ISubsystem, IUpdate
    {
        /// <summary>
        /// Subsystem name
        /// </summary>
        public string Name => nameof(MainSystem);

        /// <summary>
        /// Event called when systems initialization start.
        /// </summary>
        public OnSystemsInitializationStart OnSystemsInitializationStart;

        /// <summary>
        /// Event called when systems initialization is in progress. Listen to this event if you want to display initialization progress bar (e.g splash screen) 
        /// </summary>
        public OnSystemInitializationProgress OnSystemsInitializationProgress;

        /// <summary>
        /// Event called when systems initialization has been completed.
        /// </summary>
        public OnSystemInitializationCompleted OnSystemInitializationCompleted;

        private int _initializationCounter;

        private readonly ILogger _logger;
        private readonly List<ISubsystem> _systems;
        private readonly List<IUpdate> _updateableSystems;

        public MainSystem(ILogger logger)
        {
            _logger = logger;
            _systems = new List<ISubsystem>();
            _updateableSystems = new List<IUpdate>();

            _initializationCounter = 0;
        }

        /// <summary>
        /// True if system has been initialized, false otherwise
        /// </summary>
        public bool IsInitialized { get; set; }

        public void InitializationFailed(ILogger logger, Exception e)
        {
            // Not implemented
        }

        /// <summary>
        /// Async initialize main system and all subsystems.
        /// </summary>
        /// <param name="logger">Logger</param>
        /// <returns>Task object</returns>
        public async Task InitializeAsync(ILogger logger)
        {
            if (IsInitialized) return;

            OnSystemsInitializationStart?.Invoke();

            foreach (var system in _systems)
            {
                try
                {
                    await system.InitializeAsync(logger);

                    OnSystemsInitializationProgress?.Invoke(system.Name, (float)++_initializationCounter / _systems.Count);

                }
                catch (Exception e)
                {
                    system.InitializationFailed(_logger, e);

                }
            }

            OnSystemInitializationCompleted?.Invoke();

            IsInitialized = true;
        }

        /// <summary>
        /// Pause all subsystems (Application goes to background)
        /// </summary>
        /// <returns>Task object</returns>
        public async Task PauseSystemAsync()
        {
            foreach (var system in _systems)
            {
                await system.PauseSystemAsync();
            }
        }

        /// <summary>
        /// Resume all subsystems (Application return from background)
        /// </summary>
        /// <returns>Task object</returns>
        public async Task ResumeSystemAsync()
        {
            foreach (var system in _systems)
            {
                await system.ResumeSystemAsync();
            }
        }

        /// <summary>
        /// Gracefully shutdown all subsystems.
        /// </summary>
        /// <returns>Task object</returns>
        public async Task ShutdownAsync()
        {
            foreach (var system in _systems)
            {
                await system.ShutdownAsync();
            }
        }

        /// <summary>
        /// Add subsystem
        /// </summary>
        /// <param name="system">Subsystem to add</param>
        /// <returns>Main System object</returns>
        /// <exception cref="ArgumentNullException">Throws null exception if subsystem is null</exception>
        public MainSystem Add(ISubsystem system)
        {
            if (system == null)
                throw new ArgumentNullException(nameof(system));

            if (_systems.Contains(system))
            {
                _logger.LogWarning("System " + system.Name + " already added to the main system!", Name);
                return this;
            }

            _systems.Add(system);

            if (system is IUpdate)
            {
                _updateableSystems.Add((IUpdate)system);
            }

            return this;
        }

        /// <summary>
        /// Update all subsystem if they implement IUpdate interface
        /// </summary>
        /// <param name="deltaTime">Delta time</param>
        public void Update(float deltaTime)
        {
            foreach (var system in _updateableSystems)
            {
                system.Update(deltaTime);
            }
        }


        /// <summary>
        /// Get subsystem by type
        /// </summary>
        /// <typeparam name="T">Subsystem type</typeparam>
        /// <returns>Subsystem object or null if not exists</returns>
        public ISubsystem GetSubsystem<T>()
        {
            return _systems.Where(s => s.GetType() == typeof(T)).FirstOrDefault();
        }
    }
}
