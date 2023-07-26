using System;

namespace SuperFramework.Interfaces
{
    /// <summary>
    /// Data passed while showing specific view.
    /// </summary>
    public interface IViewData
    {

    };

    public interface IView : IInitializable
    {
        string Name { get; }

        bool IsVisible { get; }

        void Show(IViewData data = null, Action onShowStart = null, Action onShowCompleted = null);

        void Hide(Action onHideStart = null, Action onHideCompleted = null);

        void Refresh(IViewData data = null, Action onRefreshCompleted = null);

    }
}
