using UnityEngine;

[System.Serializable]
public class AdUnitSettings : ScriptableObject
{
    [Header("Android Ad Unit IDs")]
    public string androidRewardedID;
    public string androidInterstitialID;
    public string androidBannerID;

    [Header("iOS Ad Unit IDs")]
    public string iosRewardedID;
    public string iosInterstitialID;
    public string iosBannerID;

    [Header("Applovin")]
    public string applovinKey;

    [Header("Game Analytics")]
    public string secretKey;
    public string gameKey;

    [Header("Facebook Settings")]
    public string appName;
    public string appKey;
}
