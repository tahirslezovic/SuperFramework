#if RELEASE_BUILD
using System;

namespace SuperFramework.Core.Utils
{
    public enum StopwatchState
    {
        None,
        Started,
        Paused,
        Stopped,
        Finished
    }

    public sealed class Stopwatch
    {
        public event Action OnStopwatchStarted;
        public event Action OnStopwatchPaused;
        public event Action OnStopwatchStopped;

        public event Action<string> OnDisplayStopwatch;

        public string Name { get; private set; }
        public StopwatchState State { get; private set; }
        public string FormattedTime
        {
            get
            {
                if (TimeSpan.FromSeconds(_currentTime).Days > 0)
                    return TimeSpan.FromSeconds(_currentTime).ToString(@"d\d\ hh\h\ mm\m");
                else if (TimeSpan.FromSeconds(_currentTime).Hours > 0)
                    return TimeSpan.FromSeconds(_currentTime).ToString(@"hh\h\ mm\m");
                else
                    return TimeSpan.FromSeconds(_currentTime).ToString(@"m\m\ ss\s");
            }
        }
        private double _currentTime;


        public Stopwatch(string name)
        {
            Name = name;
            State = StopwatchState.None;
        }

        public void Start()
        {
            if (State == StopwatchState.Started)
                return;

            _currentTime = 0;
            State = StopwatchState.Started;
            OnStopwatchStarted?.Invoke();
        }

        public void Pause()
        {
            if (State == StopwatchState.Started)
                State = StopwatchState.Paused;

            OnStopwatchPaused?.Invoke();

        }

        public void Stop()
        {

            if (State == StopwatchState.Started || State == StopwatchState.Paused)
            {
                State = StopwatchState.Stopped;


                OnStopwatchStopped?.Invoke();
            }
        }

        public void Update(float deltaTime)
        {
            if (State == StopwatchState.Started)
            {
                _currentTime += deltaTime;


                OnDisplayStopwatch?.Invoke(FormattedTime);

               
            }
        }

    }
}
#endif