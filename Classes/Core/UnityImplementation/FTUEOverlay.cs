using SuperFramework.Interfaces;
using System;
using System.Threading.Tasks;

namespace SuperFramework.Classes.Core.UnityImplementation
{

    public class TutorialOverlayData : IViewData
    {
        public string Flow { get; set; }
    }

    /// <summary>
    /// FTUE overlay is a "holder" for all tutorial flows, ONLY FTUESubsystem is responsible for communication between FTUEOverlay and FTUESubsystem, and communication is in this way FTUESubsystem---call--->FTUEOverlay
    /// </summary>
    public class FTUEOverlay : BaseOverlay
    {
        #region Properties

        public override string Name => nameof(FTUEOverlay);

        public override int Id => gameObject.GetInstanceID();

        #endregion

        #region API

        public override Task InitializeAsync(ILogger logger = null)
        {
            if (IsInitialized)
                return Task.CompletedTask;

            logger?.Log("Initialization start", Name);

            IsInitialized = true;
            return Task.CompletedTask;
        }


        public override void Show(IViewData data = null, Action onShowStart = null, Action onShowCompleted = null)
        {
            
        }

        public override void Hide(Action onHideStart = null, Action onHideCompleted = null)
        {
            
        }

        #endregion

    }
}
