namespace SuperFramework.Interfaces
{
    /// <summary>
    /// Base interface for every overlay in the game.
    /// </summary>
    public interface IOverlay : IView
    {
        /// <summary>
        /// Overlay Id, it should be unique!
        /// </summary>
        int Id { get; }
    }
}
