using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SuperFramework.Core.Utils;
using SuperFramework.Interfaces;

namespace SuperFramework.Classes.Core.Subsystems
{
    /// <summary>
    /// Timer factory
    /// </summary>
    public class TimersSubsytem : ISubsystem, IUpdate
    {
        #region Fields

        protected List<Timer> _activeTimers;

        #endregion

        #region Properties

        public string Name => nameof(TimersSubsytem);

        public bool IsInitialized { get; protected set; }

        #endregion

        #region API

        public void InitializationFailed(ILogger logger, Exception e) => logger?.LogException("Initialization failed", e, Name);

        public Task InitializeAsync(ILogger logger = null)
        {
            _activeTimers = new List<Timer>();
            return Task.CompletedTask;
        }

        public Task PauseSystemAsync()
        {
            foreach (var timer in _activeTimers)
            {
                timer?.Pause();
            }
            return Task.CompletedTask;
        }

        public Task ResumeSystemAsync()
        {
            foreach (var timer in _activeTimers)
            {
                timer?.Resume();
            }

            return Task.CompletedTask;
        }

        public Task ShutdownAsync()
        {
            foreach (var timer in _activeTimers)
            {
                timer?.Stop();
            }
            return Task.CompletedTask;
        }

        /// <summary>
        /// Get new timer
        /// </summary>
        /// <param name="name">Timer name</param>
        /// <param name="durationInSeconds">Duration in seconds</param>
        /// <param name="timeFormat">Time format</param>
        /// <returns></returns>
        public Timer GetNewTimer(string name, double durationInSeconds, string timeFormat = null)
        {
            Timer timer = new Timer(name, durationInSeconds, timeFormat);

            _activeTimers.Add(timer);

            return timer;
        }

        /// <summary>
        /// Remove timer
        /// </summary>
        /// <param name="timer">Timer object</param>
        /// <returns>True if timer has been removed successfuly, false otherwise.</returns>
        public bool RemoveTimer(Timer timer)
        {
            int index = -1;
            for (int i = 0; i < _activeTimers.Count; i++)
            {
                if (_activeTimers[i] == timer && _activeTimers[i].Name == timer.Name)
                {
                    index = i;
                    break;

                }
            }
            if (index == -1)
                return false;


            Timer timerToRemove = _activeTimers[index];
            timerToRemove?.Stop();

            return _activeTimers.Remove(timerToRemove);
        }

        public void Update(float deltaTime)
        {
            foreach (var timer in _activeTimers)
            {
                timer?.Update(deltaTime);
            }
        }

        #endregion

    }
}

