using System;


namespace SuperFramework.Interfaces
{
    /// <summary>
    /// Network probe is used to detect internet reachability
    /// </summary>
    public interface INetworkProbe : IUpdate
    {
        /// <summary>
        /// Probe unique Id
        /// </summary>
        string Id { get; }

        /// <summary>
        /// Network url to check if it's online or no
        /// </summary>
        string Url { get; }

        /// <summary>
        /// True if this probe is online, false otherwise
        /// </summary>
        bool Online { get; }

        /// <summary>
        /// Probe implementation to check the status
        /// </summary>
        void Check();
    }
}
