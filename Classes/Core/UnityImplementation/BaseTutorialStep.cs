using SuperFramework.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UIElements;

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

        public BaseTutorialFlow ParentFlow { get; set; }
    };

    public abstract class BaseTutorialStep : MonoBehaviour
    {
        #region Fields

        protected CanvasGroup _canvasGroup;
        protected FtueStepViewData _data;


        #endregion

        #region Properties

        public abstract int StepId { get; }
        public int Id => gameObject.GetInstanceID();
        public string Name { get; protected set; }
        public bool IsVisible { get; protected set; }

        #endregion

        #region API

        public virtual void Initialize()
        {
            _canvasGroup = GetComponent<CanvasGroup>();
        }

        public virtual void Show(IViewData data = null, Action onShowStart = null, Action onShowCompleted = null)
        {
            onShowStart?.Invoke();
            onShowCompleted?.Invoke();
            _data?.AnalyticsEventStart?.Invoke();
        }

        public virtual void Hide(Action onHideStart = null, Action onHideCompleted = null)
        {
            onHideStart?.Invoke();
            onHideCompleted?.Invoke();  
            _data?.AnalyticsEventEnd?.Invoke();
        }

        public virtual void Refresh(IViewData data = null, Action onRefreshCompleted = null)
        {
            // nothing
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
