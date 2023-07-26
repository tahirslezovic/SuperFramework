using SuperFramework.Classes.Core.UnityImplementation;
using SuperFramework.Interfaces;
using System;
using System.Threading.Tasks;

namespace SuperFramework.Classes.Core.Subsystems
{
    public class TutorialSystem : ISubsystem
    {

        #region Fields

        protected ILogger _logger;
        protected TutorialOverlay _tutorialOverlay;

        #endregion

        #region Properties

        public string Name => nameof(TutorialSystem);

        public bool IsInitialized { get; protected set; }

        public TutorialFlow CurrentFlow { get; protected set; }

        /// <summary>
        /// Check if a certian Ftue is completed
        /// </summary>
        // public bool CheckFtueCompletion(TutorialFlow flow) => _ftueNakamaSystem.Sequences[flow.ToString()].Completed;

        #endregion

        #region API

        public virtual void InitializationFailed(ILogger logger, Exception e)
        {
           // Nothing
        }

        public virtual Task InitializeAsync(ILogger logger = null)
        {
            if (IsInitialized) return Task.CompletedTask;

            _logger = logger;
            IsInitialized = true;
            CurrentFlow = TutorialFlow.None;

            return Task.CompletedTask;
        }

        public virtual async void StartTutorialFlow(TutorialFlow flow)
        {
            // TODO: Check first if flow is not completed, if yes, just ignore
            //       Probably we can store ftue data through SaveGameSubsystem


            if(!IsInitialized)
            {
                _logger?.LogError("Trying to start tutorial flow, but system is not initialized!", Name);
                return;
            }
            else
            {
               await GameContext.Instance.GetSubsystem<UISubsystem>().ShowOverlay<TutorialOverlay>(new TutorialOverlayData()
                {
                    Flow = flow
                }, onInstantiated: (overlay) => { _tutorialOverlay = overlay as TutorialOverlay; });
            }
        }

        /// <summary>
        /// Move on the next step in the current flow
        /// </summary>
        public virtual void MoveOnNextStep()
        {
            
        }

        public virtual Task PauseSystemAsync()
        {
            return Task.CompletedTask;
        }

        public virtual Task ResumeSystemAsync()
        {
            return Task.CompletedTask;
        }

        public virtual Task ShutdownAsync()
        {
            return Task.CompletedTask;
        }

        #endregion

    }
}
