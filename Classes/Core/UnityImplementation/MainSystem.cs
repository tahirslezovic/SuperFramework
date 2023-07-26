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

    public sealed class MainSystem : ISubsystem, IUpdate
    {
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

        public bool IsInitialized { get; set; }

        public void InitializationFailed(ILogger logger, Exception e)
        {
            // Not implemented
        }

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

        public async Task PauseSystemAsync()
        {
            foreach (var system in _systems)
            {
                await system.PauseSystemAsync();
            }
        }

        public async Task ResumeSystemAsync()
        {
            foreach (var system in _systems)
            {
                await system.ResumeSystemAsync();
            }
        }

        public async Task ShutdownAsync()
        {
            foreach (var system in _systems)
            {
                await system.ShutdownAsync();
            }
        }

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

        public void Update(float deltaTime)
        {
            foreach (var system in _updateableSystems)
            {
                system.Update(deltaTime);
            }
        }

    
        public ISubsystem GetSubsystem<T>()
        {
            return _systems.Where(s => s.GetType() == typeof(T)).FirstOrDefault();
        }
    }
}
