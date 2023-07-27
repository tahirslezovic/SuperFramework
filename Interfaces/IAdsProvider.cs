using System.Collections.Generic;

namespace SuperFramework.Interfaces
{
    /// <summary>
    /// Base interface for each ad provider in game.
    /// Every ad provider need to implement this interface. Each Ad provider should be added through the AdsSystem.
    /// </summary>
    public interface IAdsProvider : IInitializable
    {
        string AndroidAppId { get; }
        string IOSAppId { get; }

        bool IsRewardedVideoReady(string placementName = null);
        bool IsBannerReady(string placementName = null);
        bool IsInterstitialReady(string placementName = null);

        void SetUserId(string userId);
        void ShowBanner(string placementName = null);
        void ShowInterstitial(string placementName = null);
        void ShowRewardedVideo(string placementName = null);
        void SetRewardedVideoServerParams(Dictionary<string, string> parameters);
        void ClearRewardedVideoServerParams();
    }
}
