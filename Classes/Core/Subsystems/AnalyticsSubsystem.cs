using SuperFramework.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SuperFramework.Core.Analytics
{
    /// <summary>
    /// Base implementation for Analytics System. Analytics System can have 0 or more analytic providers (IAnalyticProvider)
    /// </summary>
    public class AnalyticsSubsystem : ISubsystem
    {
        public string Name => nameof(AnalyticsSubsystem);

        public bool IsInitialized { get; protected set; }

        protected List<IAnalyticsProvider> _analyticsProviders;

        public AnalyticsSubsystem(List<IAnalyticsProvider> providers)
        {
            _analyticsProviders = providers;
        }

        public virtual void InitializationFailed(ILogger logger, Exception e)
        {
            logger.LogException("Initialization failed", e, Name);
        }

        public virtual async Task InitializeAsync(ILogger logger = null)
        {
            if (IsInitialized)
                return;

            foreach (var provider in _analyticsProviders)
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
            foreach (var provider in _analyticsProviders)
            {
                if (provider.GetType() == typeof(T))
                {
                    return (T)provider;
                }
            }
            return default(T);
        }


        public virtual void LogEvent(string eventName, Dictionary<string, object> properties = null)
        {
            foreach (var provider in _analyticsProviders)
            {
                provider.LogEvent(eventName, properties);
            }
        }
        public virtual void AppLaunch(Dictionary<string, object> properties = null)
        {
            foreach (var provider in _analyticsProviders)
            {
                provider.AppLaunch(properties);
            }
        }
        public virtual void SignUp(string provider)
        {
            foreach (var p in _analyticsProviders)
            {
                p.SignUp(provider);
            }
        }
        public virtual void InAppPurchase(Dictionary<string, object> properties)
        {
            foreach (var provider in _analyticsProviders)
            {
                provider.InAppPurchase(properties);
            }
        }
        public virtual void GameStart(Dictionary<string, object> properties)
        {
            foreach (var provider in _analyticsProviders)
            {
                provider.GameStart(properties);
            }
        }
        public virtual void GameEnd(Dictionary<string, object> properties)
        {
            foreach (var provider in _analyticsProviders)
            {
                provider.GameEnd(properties);
            }
        }
        public virtual void TutorialBegin(string tutorialId = null)
        {
            foreach (var provider in _analyticsProviders)
            {
                provider.TutorialBegin(tutorialId);
            }
        }
        public virtual void TutorialStepCompleted(string tutorialId = null, string stepId = null, Dictionary<string, object> properties = null)
        {
            foreach (var provider in _analyticsProviders)
            {
                provider.TutorialStepCompleted(tutorialId, stepId, properties);
            }
        }
        public virtual void TutorialEnd(string tutorialId = null)
        {
            foreach (var provider in _analyticsProviders)
            {
                provider.TutorialEnd(tutorialId);
            }
        }

        public virtual void TutorialSkip(string tutorialId = null, string stepId = null)
        {
            foreach (var provider in _analyticsProviders)
            {
                provider.TutorialSkip(tutorialId, stepId);
            }
        }

        public virtual void ScreenViewed(string screenName, string screenBefore = null)
        {
            foreach (var provider in _analyticsProviders)
            {
                provider.ScreenViewed(screenName, screenBefore);
            }
        }
        public virtual void SetUserId(string userId)
        {
            foreach (var provider in _analyticsProviders)
            {
                provider.SetUserId(userId);
            }
        }
        public virtual void SendException(Exception e)
        {
            foreach (var provider in _analyticsProviders)
            {
                provider.SendException(e);
            }
        }
    }
}
