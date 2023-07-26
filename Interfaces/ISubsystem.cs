using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperFramework.Interfaces
{
    public interface ISubsystem : IInitializable
    {
        string Name { get; }

        Task ShutdownAsync();

        Task PauseSystemAsync();

        Task ResumeSystemAsync();

    }
}
