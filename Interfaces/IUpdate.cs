namespace SuperFramework.Interfaces
{
    /// <summary>
    /// Updateable interface for specific system which need to have update() method which is called by MainSystem
    /// </summary>
    public interface IUpdate
    {
        void Update(float deltaTime);
    }
}
