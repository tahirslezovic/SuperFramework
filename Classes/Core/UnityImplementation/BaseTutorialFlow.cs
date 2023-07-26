using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SuperFramework.Classes.Core.UnityImplementation
{

    public enum TutorialFlow
    {
        None = -1,
        // play_button or whatever tutorial flow is called
    }

    public class BaseTutorialFlowData
    {
        public int CurrentStep { get; set; }
    }

    public abstract class BaseTutorialFlow : MonoBehaviour
    {
        #region Fields

        protected BaseTutorialFlowData _tutorialFlowData;
        protected List<BaseTutorialStep> _tutorialSteps;
        protected abstract Dictionary<int, FtueStepViewData> _tutorialStepDatas { get; }

        #endregion

        #region Properties
        public abstract TutorialFlow FtueFlow { get; }

        /// <summary>
        /// Check if ftue flow sequence is completed, false if is not or ftue is not initialized
        /// </summary>
        //public bool IsFlowCompleted => _flowData != null && _flowData.FtueSequence.Completed;

        public int StartingStepId { get; private set; }

        #endregion

        #region API

        public virtual void Initialize(BaseTutorialFlowData tutorialFlowData)
        {
            _tutorialFlowData = tutorialFlowData;
            //StartingStepId = Get from save game subsystem, so player can continue from this step
            _tutorialSteps = GetComponentsInChildren<BaseTutorialStep>().ToList();
            for (int i = 0; i < _tutorialSteps.Count; i++)
            {
                _tutorialSteps[i].Initialize();
            }
        }

        public virtual void Refresh(BaseTutorialFlowData tutorialFlowData)
        {
            _tutorialFlowData = tutorialFlowData;
        }


        public virtual void ShowCurrentStep()
        {
              if (_tutorialFlowData == null && _tutorialStepDatas == null)
              {

                this.GetLogger().LogError("Flow data or ftue steps are null!");
                  return;
              }
              int currentStep = _tutorialFlowData.CurrentStep;

              if (currentStep > 0 && _tutorialSteps[currentStep - 1].IsVisible)
              {
                  _tutorialSteps[currentStep - 1].Hide(onHideCompleted: () =>
                  {
                      _tutorialSteps[currentStep].Show(_tutorialStepDatas[currentStep]);
                  });
              }
              else
              {
                  _tutorialSteps[currentStep].Show(_tutorialStepDatas[currentStep]);

              }
        }

        public virtual void HideCurrentStep()
        {
            _tutorialSteps[_tutorialFlowData.CurrentStep].Hide();
        }

        #endregion
    }
}
