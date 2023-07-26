using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperFramework.Interfaces
{
    /// <summary>
    /// Base interface for every subsystem in the game.
    /// </summary>
    public interface ISubsystem : IInitializable
    {
        /// <summary>
        /// Subsystem name, it should be unique!
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Method called when this subsystem need gracefully to be shutted down.
        /// </summary>
        /// <returns>Task</returns>
        Task ShutdownAsync();

        /// <summary>
        /// Method called when application has been paused (When application goes to the background)
        /// </summary>
        /// <returns>Task</returns>
        Task PauseSystemAsync();

        /// <summary>
        /// Method called when application has been resumed (when application returns back to foreground)
        /// </summary>
        /// <returns>Task</returns>
        Task ResumeSystemAsync();

    }
}
