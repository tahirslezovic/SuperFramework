using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SuperFramework.Classes.Core.UnityImplementation
{

    public enum FTUEFlow
    {
        None = -1,
        // play_button or whatever tutorial flow is called
    }

    public class BaseFTUEFlowData
    {
        public int CurrentStep { get; set; }
    }

    public abstract class BaseFTUEFlow : MonoBehaviour
    {
        #region Fields

        protected BaseFTUEFlowData _ftueFlowData;
        protected List<BaseFTUEStep> _ftueSteps;
        protected abstract Dictionary<int, FtueStepViewData> _tutorialStepDatas { get; }

        #endregion

        #region Properties
        public abstract FTUEFlow FtueFlow { get; }

        /// <summary>
        /// Check if ftue flow sequence is completed, false if is not or ftue is not initialized
        /// </summary>
        //public bool IsFlowCompleted => _flowData != null && _flowData.FtueSequence.Completed;

        public int StartingStepId { get; private set; }

        #endregion

        #region API

        public virtual void Initialize(BaseFTUEFlowData ftueFlowData)
        {
            _ftueFlowData = ftueFlowData;
            //StartingStepId = Get from save game subsystem, so player can continue from this step
            _ftueSteps = GetComponentsInChildren<BaseFTUEStep>().ToList();
            for (int i = 0; i < _ftueSteps.Count; i++)
            {
                _ftueSteps[i].InitializeAsync();
            }
        }

        public virtual void Refresh(BaseFTUEFlowData ftueFlowData)
        {
            _ftueFlowData = ftueFlowData;
        }


        public virtual void ShowCurrentStep()
        {
              if (_ftueFlowData == null && _tutorialStepDatas == null)
              {

                this.GetLogger().LogError("Flow data or ftue steps are null!");
                  return;
              }
              int currentStep = _ftueFlowData.CurrentStep;

              if (currentStep > 0 && _ftueSteps[currentStep - 1].IsVisible)
              {
                _ftueSteps[currentStep - 1].Hide(onHideCompleted: () =>
                  {
                      _ftueSteps[currentStep].Show(_tutorialStepDatas[currentStep]);
                  });
              }
              else
              {
                _ftueSteps[currentStep].Show(_tutorialStepDatas[currentStep]);

              }
        }

        public virtual void HideCurrentStep()
        {
            _ftueSteps[_ftueFlowData.CurrentStep].Hide();
        }

        #endregion
    }
}
