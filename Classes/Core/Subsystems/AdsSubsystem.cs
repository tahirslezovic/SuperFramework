using SuperFramework.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SuperFramework.Core.Ads
{
    /// <summary>
    /// Base implementation for ads system. Ads system can hold 0 or more ads providers (IAdProvider)
    /// </summary>
    public class AdsSubsystem : ISubsystem
    {
        public string Name => nameof(AdsSubsystem);

        public bool IsInitialized { get; protected set; }

        protected List<IAdsProvider> _adsProviders;

        public AdsSubsystem(List<IAdsProvider> providers)
        {
            _adsProviders = providers;
        }

        public virtual void InitializationFailed(ILogger logger, Exception e)
        {
            logger?.LogError("Initialization failed", Name);
        }

        public virtual async Task InitializeAsync(ILogger logger = null)
        {
            if (IsInitialized)
                return;

            foreach (var provider in _adsProviders)
            {
                await provider.InitializeAsync(logger);
            }

            IsInitialized = true;
        }

        public virtual Task PauseSystemAsync()
        {
            // Nothing
            return Task.CompletedTask;
        }

        public virtual Task ResumeSystemAsync()
        {
            // Nothing
            return Task.CompletedTask;
        }

        public virtual Task ShutdownAsync()
        {
            // Nothing
            return Task.CompletedTask;
        }

        public virtual T GetProvider<T>()
        {
            foreach (var provider in _adsProviders)
            {
                if (provider.GetType() == typeof(T))
                {
                    return (T)provider;
                }
            }
            return default(T);
        }
    }
}
