using System;

namespace SuperFramework.Interfaces
{
    /// <summary>
    /// Data passed while showing specific view.
    /// </summary>
    public interface IViewData
    {

    };

    /// <summary>
    /// Base interface for every view in the game
    /// </summary>
    public interface IView : IInitializable
    {
        /// <summary>
        /// View name. It should be unique!
        /// </summary>
        string Name { get; }

        /// <summary>
        /// True if this view is visible, false otherwise.
        /// </summary>
        bool IsVisible { get; }

        /// <summary>
        /// Show view on the scree
        /// </summary>
        /// <param name="data"></param>
        /// <param name="onShowStart"></param>
        /// <param name="onShowCompleted"></param>
        void Show(IViewData data = null, Action onShowStart = null, Action onShowCompleted = null);

        /// <summary>
        /// Hide view.
        /// </summary>
        /// <param name="onHideStart"></param>
        /// <param name="onHideCompleted"></param>
        void Hide(Action onHideStart = null, Action onHideCompleted = null);

        /// <summary>
        /// Refresh view with specific data.
        /// </summary>
        /// <param name="data"></param>
        /// <param name="onRefreshCompleted"></param>
        void Refresh(IViewData data = null, Action onRefreshCompleted = null);

    }
}
