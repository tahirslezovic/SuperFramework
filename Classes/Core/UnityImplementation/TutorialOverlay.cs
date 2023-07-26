using SuperFramework.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperFramework.Classes.Core.UnityImplementation
{

    public class TutorialOverlayData : IViewData
    {
        public TutorialFlow Flow { get; set; }
    }

    /// <summary>
    /// Tutorial overlay is a "holder" for all tutorial flows, ONLY TutorialSystem is responsible for communication between TutorialOverlay and TutorialSystem, and communication is in this way TutorialSystem---call--->TutorialOverlay
    /// </summary>
    public class TutorialOverlay : BaseOverlay
    {

        #region Fields
        
        #endregion

        #region Properties

        public override string Name => nameof(TutorialOverlay);

        public override int Id => gameObject.GetInstanceID();

        #endregion

        #region API

        public override Task InitializeAsync(ILogger logger = null)
        {
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
