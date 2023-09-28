using SuperFramework.Interfaces;
using System;

namespace SuperFramework.Classes.Core.UnityImplementation
{
    public class FtueStepViewData : IViewData
    {
        public int Index { get; set; }
        public string TitleText { get; set; }
        public string DetailsText { get; set; }
        public string NextScene { get; set; }

        public Action AnalyticsEventStart { get; set; }
        public Action AnalyticsEventEnd { get; set; }

        public BaseFTUEFlow ParentFlow { get; set; }
    };

    /// <summary>
    /// Base overlay for every FTUE step
    /// </summary>
    public abstract class BaseFTUEStep : BaseOverlay
    {
        #region Fields

        protected FtueStepViewData _data;

        #endregion

        #region Properties

        /// <summary>
        /// FTUE Step id
        /// </summary>
        public abstract int StepId { get; }

        #endregion

        #region API

        public override void Show(IViewData data = null, Action onShowStart = null, Action onShowCompleted = null)
        {
            _data = (FtueStepViewData)data;
            _data?.AnalyticsEventStart?.Invoke();

            base.Show(data, onShowStart, onShowCompleted);
        }

        public override void Hide(Action onHideStart = null, Action onHideCompleted = null)
        {
            _data?.AnalyticsEventEnd?.Invoke();
            base.Hide(onHideStart, onHideCompleted);
        }

        #endregion

        #region Private / Protected Methods


        protected virtual void OnFtueStepCompleted()
        {
            // GameCoordinator.Instance.FtueSystem.UpdateCurrentFtueFlowStep(_data.Index + 1,
            //   () => { GameCoordinator.Instance.FtueSystem.MoveOnNextStep(); });
        }

        #endregion
    }
}
