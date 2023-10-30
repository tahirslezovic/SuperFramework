using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SuperFramework.Interfaces;

namespace SuperFramework.Classes.Core.Subsystems
{
    /// <summary>
    /// Network monitor subsystem is responsible 
    /// </summary>
    public class NetworkMonitorSubsystem : ISubsystem, IUpdate
    {

        #region Fields

        protected ILogger _logger;
        protected bool _probeCheckStarted;
        protected float _passedTime;
        protected float _checkDelay;
        protected bool _previousOnlineStatus;

        #endregion

        #region Properties

        public string Name => nameof(NetworkMonitorSubsystem);

        /// <summary>
        /// True if subsystem is initialized, false otherwise
        /// </summary>
        public bool IsInitialized { get; protected set; }

        /// <summary>
        /// List of network probes
        /// </summary>
        public List<INetworkProbe> Probes { get; protected set; }

        /// <summary>
        /// Get last online status.
        /// </summary>
        public bool Online => _previousOnlineStatus;

        /// <summary>
        /// Event fired when online status has been changed, it will contains previous online state and new state, so it will be pair(online->offline, offline->online), it will never be in pair(online->online, offline->offline)
        /// </summary>
        public Action<bool,bool> OnlineStatusChanged { get; set; }

        #endregion

        #region Constructor

        /// <summary>
        /// 
        /// </summary>
        /// <param name="probes">List of probles</param>
        /// <param name="checkDelaySeconds">After how many seconds subsystem should check online status</param>
        public NetworkMonitorSubsystem(List<INetworkProbe> probes, float checkDelaySeconds= 5)
        {
            Probes = probes;
            _passedTime = 0f;
            _probeCheckStarted = true;
            _checkDelay = checkDelaySeconds;
            _previousOnlineStatus = true;
        }

        #endregion



        #region API

        public void InitializationFailed(ILogger logger, Exception e) => logger?.LogException("Initialization failed", e, Name);

        public Task InitializeAsync(ILogger logger = null)
        {
            if (IsInitialized)
                return Task.CompletedTask;

            _logger = logger;


            IsInitialized = true;
            return Task.CompletedTask;
        }

        public Task PauseSystemAsync()
        {
            _probeCheckStarted = false;
            return Task.CompletedTask;
        }

        public Task ResumeSystemAsync()
        {
            _probeCheckStarted = true;
            return Task.CompletedTask;
        }

        public Task ShutdownAsync()
        {
            _probeCheckStarted = false;
            return Task.CompletedTask;
        }

        public void Update(float deltaTime)
        {
            if(_probeCheckStarted)
            {
                // First we want to run update for all probes
                for (int i=0;i<Probes.Count;i++)
                {
                    Probes[i].Update(deltaTime);
                }

                _passedTime += deltaTime;

                if(_passedTime >= _checkDelay)
                {
                    _passedTime = 0f;

                    bool online = true;
                    for(int i=0;i<Probes.Count;i++)
                    {
                        if(!Probes[i].Online)
                        {
                            online = false;
                            break;
                        }
                    }

                    // If one of the probes is different than previous state, we should inform everyone
                    if(online != _previousOnlineStatus)
                    {
                        // Fire event that online status changed from previous->current
                        OnlineStatusChanged?.Invoke(_previousOnlineStatus, online);

                        _previousOnlineStatus = online;
                    }
                }
            }
        }

        #endregion
    }
}
