using SuperFramework.Interfaces;
using System;
using System.Threading.Tasks;

namespace SuperFramework.Classes.Core.UnityImplementation
{

    public class TutorialOverlayData : IViewData
    {
        public FTUEFlow Flow { get; set; }
    }

    /// <summary>
    /// FTUE overlay is a "holder" for all tutorial flows, ONLY TutorialSystem is responsible for communication between TutorialOverlay and TutorialSystem, and communication is in this way TutorialSystem---call--->FTUEOverlay
    /// </summary>
    public class FTUEOverlay : BaseOverlay
    {

        #region Fields
        
        #endregion

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
