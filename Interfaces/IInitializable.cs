using System;
using System.Threading.Tasks;

namespace SuperFramework.Interfaces
{
    /// <summary>
    /// Base interface for each object which can be initialized
    /// </summary>
    public interface IInitializable
    {
        /// <summary>
        /// True if object is initialized, false otherwise
        /// </summary>
        bool IsInitialized { get; }

        /// <summary>
        /// Async object initialization
        /// </summary>
        /// <param name="logger">Logger which can be passed so you can log initialization cycle</param>
        /// <returns>Task object</returns>
        Task InitializeAsync(ILogger logger = null);

        /// <summary>
        /// Method called when initialization has failed, usually this method is called from MainSystem
        /// </summary>
        /// <param name="logger">Logger which can be passed so you can log why initializaion has failed</param>
        /// <param name="e">Exception raised during initializations</param>
        void InitializationFailed(ILogger logger, Exception e);
    }
}
