using System;
using System.Threading.Tasks;

namespace SuperFramework.Interfaces
{
    public interface IInitializable
    {
        bool IsInitialized { get; }
        Task InitializeAsync(ILogger logger = null);
        void InitializationFailed(ILogger logger, Exception e);
    }
}
