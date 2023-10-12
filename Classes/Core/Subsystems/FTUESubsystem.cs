using SuperFramework.Classes.Core.UnityImplementation;
using SuperFramework.Interfaces;
using System;
using System.Threading.Tasks;

namespace SuperFramework.Classes.Core.Subsystems
{
    /// <summary>
    /// Base implementation for all FTUE's in the game.
    /// </summary>
    public abstract class FTUESubsystem : ISubsystem
    {
       
        #region Fields

        protected ILogger _logger;
        protected FTUEOverlay _ftueOverlay;

        #endregion

        #region Properties

        public string Name => nameof(FTUESubsystem);

        public bool IsInitialized { get; protected set; }

        public FTUEFlow CurrentFlow { get; protected set; }

        /// <summary>
        /// Check if a certian Ftue is completed
        /// </summary>
        // public bool CheckFtueCompletion(TutorialFlow flow) => _ftueNakamaSystem.Sequences[flow.ToString()].Completed;

        #endregion

        #region API

        public virtual void InitializationFailed(ILogger logger, Exception e) => logger?.LogException("Initialization failed", e, Name);

        public virtual Task InitializeAsync(ILogger logger = null)
        {
            if (IsInitialized) return Task.CompletedTask;

            _logger = logger;
            CurrentFlow = FTUEFlow.None;

            IsInitialized = true;
            return Task.CompletedTask;
        }

        public virtual async void StartTutorialFlow(FTUEFlow flow)
        {
            if(!IsInitialized)
            {
                _logger?.LogError("Trying to start ftue flow, but system is not initialized!", Name);
                return;
            }
            else
            {
               await GameContext.Instance.GetSubsystem<UISubsystem>().ShowOverlay<FTUEOverlay>(new TutorialOverlayData()
                {
                    Flow = flow
                }, onInstantiated: (overlay) => { _ftueOverlay = overlay as FTUEOverlay; });
            }
        }

        /// <summary>
        /// Move on the next step in the current flow
        /// </summary>
        public virtual void MoveOnNextStep()
        {
            
        }

        public virtual Task PauseSystemAsync() => Task.CompletedTask;

        public virtual Task ResumeSystemAsync() => Task.CompletedTask;

        public virtual Task ShutdownAsync() => Task.CompletedTask;

        #endregion

    }
}
