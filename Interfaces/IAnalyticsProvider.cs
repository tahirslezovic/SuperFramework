using System;
using System.Collections.Generic;

namespace SuperFramework.Interfaces
{
    /// <summary>
    /// Base interface for each analytics provider in the game.s
    /// Every analytics provider should implement this interface, and it should be added through the AnalyticsSystem.
    /// </summary>
    public interface IAnalyticsProvider : IInitializable
    {
        void LogEvent(string eventName, Dictionary<string, object> properties = null);
        void AppLaunch(Dictionary<string, object> properties = null);
        void SignUp(string provider);
        void InAppPurchase(Dictionary<string, object> properties);
        void GameStart(Dictionary<string, object> properties);
        void GameEnd(Dictionary<string, object> properties);
        void TutorialBegin(string tutorialId = null);
        void TutorialStepCompleted(string tutorialId = null, string stepId = null, Dictionary<string, object> properties = null);
        void TutorialEnd(string tutorialId = null);
        void TutorialSkip(string tutorialId = null, string stepId = null);
        void ScreenViewed(string screenName, string screenBefore = null);
        void SetUserId(string userId);
        void SendException(Exception e);
    }
}
