namespace SuperFramework.Interfaces
{
    /// <summary>
    /// Base interface for each pop-up in the game.
    /// </summary>
    public interface IPopUp : IView
    {
        /// <summary>
        /// Pop-up id, it should be unique!
        /// </summary>
        int Id { get; }
    }
}
