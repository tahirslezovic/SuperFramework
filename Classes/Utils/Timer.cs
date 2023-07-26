#if RELEASE_BUILD
using SuperFramework.Interfaces;
using System;

namespace SuperFramework.Core.Utils
{
    public enum TimerState
    {
        None,
        Started,
        Paused,
        Stopped,
        Finished
    }

    /// <summary>
    /// Simple countdown timer class. 
    /// </summary>
    public sealed class Timer : IUpdate
    {

        public event Action OnTimerStarted;
        public event Action OnTimerPaused;
        public event Action OnTimerStopped;
        public event Action OnTimerFinished;

        /// <summary>
        /// Display time in 3 different format: left time, scaled time and formatted time
        /// </summary>
        public event Action<double, double, string> OnDisplayTimer;

        /// <summary>
        /// Unique timer name
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Total seconds 
        /// </summary>
        public double TotalSeconds { get; private set; }

        public TimerState State { get; private set; }

        /// <summary>
        /// Get how much seconds left till timer finish
        /// </summary>
        public double LeftTime
        {
            get
            {
                // Prevent from displaying -1 in last frame
                return (TotalSeconds - _currentTime) < 0 ? 0 : TotalSeconds - _currentTime;
            }
        }

        /// <summary>
        /// Get formatted time (hh mm ss)
        /// </summary>
        public string FormattedTime
        {
            get
            {
                if (string.IsNullOrEmpty(_timeFormat))
                {
                    if (TimeSpan.FromSeconds(LeftTime).Days > 0)
                        return TimeSpan.FromSeconds(LeftTime).ToString(@"d\d\ hh\h\ mm\m");
                    else if (TimeSpan.FromSeconds(LeftTime).Hours > 0)
                        return TimeSpan.FromSeconds(LeftTime).ToString(@"hh\h\ mm\m");
                    else
                        return TimeSpan.FromSeconds(LeftTime).ToString(@"m\m\ ss\s");
                }

                return _timeFormat;
             
            }
        }

        /// <summary>
        /// Get scaled time from 0 to 1
        /// </summary>
        public double ScaledTime
        {
            get
            {
                return _currentTime / TotalSeconds;
            }
        }

        private double _currentTime;
        private string _timeFormat;

        public Timer(string name, double durationInSeconds, string timeFormat = null)
        {
            TotalSeconds = durationInSeconds;
            Name = name;
            State = TimerState.None;
            _currentTime = 0f;
            _timeFormat = timeFormat;
        }
        public void Start()
        {
            if (State == TimerState.Started)
                return;

            _currentTime = 0;
            State = TimerState.Started;
            OnTimerStarted?.Invoke();
        }

        public void Pause()
        {
            if (State == TimerState.Started)
                State = TimerState.Paused;

            OnTimerPaused?.Invoke();
        }

        public void Stop()
        {
            if (State == TimerState.Started || State == TimerState.Paused)
            {
                State = TimerState.Stopped;


                OnTimerStopped?.Invoke();
            }
        }

        /// <summary>
        /// Set timer duration
        /// </summary>
        /// <param name="newDuration">If newDuration is -1, it will keep timer duration same like it was initialized. </param>
        public void Restart(double newDuration = -1)
        {
            State = TimerState.Started;
            _currentTime = 0;
            TotalSeconds = newDuration == -1 ? TotalSeconds : newDuration;
        }

        public void Update(float deltaTime)
        {
            if (State == TimerState.Started)
            {
                _currentTime += deltaTime;

                if (_currentTime >= TotalSeconds)
                {
                    State = TimerState.Finished;

                    OnDisplayTimer?.Invoke(LeftTime, ScaledTime, FormattedTime);

                    OnTimerFinished?.Invoke();
                }
                else
                {
                    OnDisplayTimer?.Invoke(LeftTime, ScaledTime, FormattedTime);
                }
            }
        }
    }

}
#endif