//using System;
//using System.Collections;
////using AdjustSdk;
//using Unity.Services.RemoteConfig;
//using UnityEngine;

//namespace BorderAgent.Shared.Managers
//{
//    public class ADManager : MonoBehaviour
//    {
//        private static ADManager instance;
//        private static readonly object padlock = new object();

//        private const string MaxSdkKey =
//            "_KvBgxvuNtcbKT1yX2tzh4U0WGIEkwXEtTAQCi1flkTqssGVxnKkgkm_VKi7TTT54C8D_3mnJqLMt3Qi6oSPC6";

//#if UNITY_IOS
//        private const string BannerAdUnitId = "a599ce49d8e67786";

//        private const string InterstitialAdUnitId = "ENTER_IOS_INTERSTITIAL_AD_UNIT_ID_HERE";
//        private const string RewardedAdUnitId = "ENTER_IOS_REWARD_AD_UNIT_ID_HERE";
//#else
//        private const string InterstitialAdUnitId = "8e1d7c2d021390d0";
//        private const string RewardedAdUnitId = "80bb76790f7b5ab4";
//        private const string BannerAdUnitId = "a599ce49d8e67786";
//#endif

//        private bool isBannerAdEnabled = true;
//        private bool isIntersitialAdEnabled = true;
//        private bool isRewardedAdEnabled = true;
//        private int intersitialTimer = 180;

//        private int interstitialRetryAttempt;
//        private int rewardedRetryAttempt;
//        private bool isBannerAdLoaded;
//        //private AdjustConfig _adjustConfig;

//        private Action rewardedAdCallback;
//        public event Action OnInterstitial;

//        public bool isInterstitialTimePassed;

//        public static ADManager Instance
//        {
//            get
//            {
//                if (instance == null)
//                {
//                }

//                return instance;
//            }
//        }

//        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
//        private static void Initialize()
//        {
//            if (instance == null)
//            {
//                GameObject adManagerObject = new GameObject("ADManager");
//                instance = adManagerObject.AddComponent<ADManager>();
//                DontDestroyOnLoad(adManagerObject);
//            }
//        }

//        public void NoAds()
//        {
//            isBannerAdEnabled = false;
//            isIntersitialAdEnabled = false;
//        }

//        private void Awake()
//        {
//            lock (padlock)
//            {
//                if (instance == null)
//                {
//                    instance = this;
//                    DontDestroyOnLoad(this.gameObject);

//                    InitializeSDKs();
//                }
//                else if (instance != this)
//                {
//                    Destroy(this.gameObject);
//                }
//            }

//            var rcBannerKey = $"isBannerAdEnabled";
//            var rcIntersitialKey = $"isIntersitialAdEnabled";
//            var rcRewardedKey = $"isRewardedAdEnabled";
//            var rcIntersitialTimerKey = $"intersitialTimer";

//            if (RemoteConfigService.Instance.appConfig.HasKey(rcBannerKey))
//            {
//                isBannerAdEnabled = RemoteConfigService.Instance.appConfig.GetBool(rcBannerKey);
//            }

//            if (RemoteConfigService.Instance.appConfig.HasKey(rcIntersitialKey))
//            {
//                isIntersitialAdEnabled = RemoteConfigService.Instance.appConfig.GetBool(rcIntersitialKey);
//            }

//            if (RemoteConfigService.Instance.appConfig.HasKey(rcRewardedKey))
//            {
//                isRewardedAdEnabled = RemoteConfigService.Instance.appConfig.GetBool(rcRewardedKey);
//            }

//            if (RemoteConfigService.Instance.appConfig.HasKey(rcIntersitialTimerKey))
//            {
//                intersitialTimer = RemoteConfigService.Instance.appConfig.GetInt(rcIntersitialTimerKey);
//            }
//        }

//        private void OnEnable()
//        {
//            StartCoroutine(InterstitialCounter());
//        }

//        private void OnLevelLoaded()
//        {
//            ShowBanner();
//        }

//        private void OnLevelUnloaded()
//        {
//            HideBanner();
//        }


//        private void InitializeSDKs()
//        {
//            MaxSdkCallbacks.OnSdkInitializedEvent += OnMaxSdkInitialized;

//            // MaxSdk.SetSdkKey(MaxSdkKey);
//            MaxSdk.InitializeSdk();
//        }

//        private void OnMaxSdkInitialized(MaxSdkBase.SdkConfiguration sdkConfiguration)
//        {
//            //_adjustConfig = new AdjustConfig("bazgyfucawow", AdjustEnvironment.Production)
//            //{
//            //    FbAppId = "521289710326635"
//            //};

//            //Adjust.InitSdk(_adjustConfig);

//            InitializeInterstitialAds();
//            InitializeRewardedAds();
//            InitializeBannerAds();
//        }

//        #region Interstitial Ads

//        private void InitializeInterstitialAds()
//        {
//            MaxSdkCallbacks.Interstitial.OnAdLoadedEvent += OnInterstitialLoadedEvent;
//            MaxSdkCallbacks.Interstitial.OnAdLoadFailedEvent += OnInterstitialFailedEvent;
//            MaxSdkCallbacks.Interstitial.OnAdDisplayFailedEvent += OnInterstitialFailedToDisplayEvent;
//            MaxSdkCallbacks.Interstitial.OnAdHiddenEvent += OnInterstitialHiddenEvent;
//            MaxSdkCallbacks.Interstitial.OnAdRevenuePaidEvent += OnInterstitialRevenuePaidEvent;

//            LoadInterstitial();
//        }

//        private void LoadInterstitial()
//        {
//            MaxSdk.LoadInterstitial(InterstitialAdUnitId);
//        }

//        public bool IsInterstitialReady()
//        {
//            return MaxSdk.IsInterstitialReady(InterstitialAdUnitId);
//        }


//        public void ShowInterstitial()
//        {
//            if (!isIntersitialAdEnabled || !isInterstitialTimePassed)
//            {
//                return;
//            }

//            if (IsInterstitialReady())
//            {
//                isInterstitialTimePassed = false;
//                OnInterstitial.Invoke();
//                MaxSdk.ShowInterstitial(InterstitialAdUnitId);
//            }
//            else
//            {
//            }
//        }

//        private void OnInterstitialLoadedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
//        {
//            interstitialRetryAttempt = 0;
//        }

//        private void OnInterstitialFailedEvent(string adUnitId, MaxSdkBase.ErrorInfo errorInfo)
//        {
//            interstitialRetryAttempt++;
//            double retryDelay = Math.Pow(2, Math.Min(6, interstitialRetryAttempt));
//            Invoke("LoadInterstitial", (float)retryDelay);
//        }

//        private void OnInterstitialFailedToDisplayEvent(string adUnitId, MaxSdkBase.ErrorInfo errorInfo,
//            MaxSdkBase.AdInfo adInfo)
//        {
//            LoadInterstitial();
//        }

//        private void OnInterstitialHiddenEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
//        {
//            LoadInterstitial();
//        }

//        IEnumerator InterstitialCounter()
//        {
//            while (true)
//            {
//                yield return new WaitForSeconds(intersitialTimer);
//                isInterstitialTimePassed = true;
//                //ShowInterstitial();
//            }
//        }

//        private void OnInterstitialRevenuePaidEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
//        {
//            //TrackAdRevenue(adInfo);
//        }

//        #endregion

//        #region Rewarded Ads

//        private void InitializeRewardedAds()
//        {
//            MaxSdkCallbacks.Rewarded.OnAdLoadedEvent += OnRewardedAdLoadedEvent;
//            MaxSdkCallbacks.Rewarded.OnAdLoadFailedEvent += OnRewardedAdFailedEvent;
//            MaxSdkCallbacks.Rewarded.OnAdDisplayFailedEvent += OnRewardedAdFailedToDisplayEvent;
//            MaxSdkCallbacks.Rewarded.OnAdDisplayedEvent += OnRewardedAdDisplayedEvent;
//            MaxSdkCallbacks.Rewarded.OnAdClickedEvent += OnRewardedAdClickedEvent;
//            MaxSdkCallbacks.Rewarded.OnAdHiddenEvent += OnRewardedAdHiddenEvent;
//            MaxSdkCallbacks.Rewarded.OnAdReceivedRewardEvent += OnRewardedAdReceivedRewardEvent;
//            MaxSdkCallbacks.Rewarded.OnAdRevenuePaidEvent += OnRewardedAdRevenuePaidEvent;

//            LoadRewardedAd();
//        }

//        private void LoadRewardedAd()
//        {
//            MaxSdk.LoadRewardedAd(RewardedAdUnitId);
//        }


//        public bool IsRewardedAdReady()
//        {
//            return MaxSdk.IsRewardedAdReady(RewardedAdUnitId);
//        }


//        public void ShowRewardedAd(Action onRewarded)
//        {
//            //onRewarded?.Invoke();

//            //return;

//            if (!isRewardedAdEnabled)
//            {
//                onRewarded?.Invoke();
//                return;
//            }

//            if (IsRewardedAdReady())
//            {
//                rewardedAdCallback = onRewarded;
//                MaxSdk.ShowRewardedAd(RewardedAdUnitId);
//            }
//            else
//            {
//                LoadRewardedAd();
//            }
//        }

//        private void OnRewardedAdLoadedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
//        {
//            rewardedRetryAttempt = 0;
//        }

//        private void OnRewardedAdFailedEvent(string adUnitId, MaxSdkBase.ErrorInfo errorInfo)
//        {
//            rewardedRetryAttempt++;
//            double retryDelay = Math.Pow(2, Math.Min(6, rewardedRetryAttempt));
//            Invoke("LoadRewardedAd", (float)retryDelay);
//        }

//        private void OnRewardedAdFailedToDisplayEvent(string adUnitId, MaxSdkBase.ErrorInfo errorInfo,
//            MaxSdkBase.AdInfo adInfo)
//        {
//            //Debug.LogError($"Rewarded ad failed to display with error code: {errorInfo.Code}. Loading next ad.");
//            LoadRewardedAd();
//        }

//        private void OnRewardedAdDisplayedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
//        {
//        }

//        private void OnRewardedAdClickedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
//        {
//        }

//        private void OnRewardedAdHiddenEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
//        {
//            LoadRewardedAd();
//        }

//        private void OnRewardedAdReceivedRewardEvent(string adUnitId, MaxSdk.Reward reward, MaxSdkBase.AdInfo adInfo)
//        {
//            rewardedAdCallback?.Invoke();
//            rewardedAdCallback = null;
//        }

//        private void OnRewardedAdRevenuePaidEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
//        {
//            //TrackAdRevenue(adInfo);
//        }

//        #endregion

//        #region Banner Ads

//        private void InitializeBannerAds()
//        {
//            MaxSdkCallbacks.Banner.OnAdLoadedEvent += OnBannerAdLoadedEvent;
//            MaxSdkCallbacks.Banner.OnAdLoadFailedEvent += OnBannerAdFailedEvent;
//            MaxSdkCallbacks.Banner.OnAdClickedEvent += OnBannerAdClickedEvent;
//            MaxSdkCallbacks.Banner.OnAdRevenuePaidEvent += OnBannerAdRevenuePaidEvent;

//            MaxSdk.CreateBanner(BannerAdUnitId, MaxSdkBase.BannerPosition.BottomCenter);

//            MaxSdk.SetBannerBackgroundColor(BannerAdUnitId, Color.black);
//        }

//        public void ShowBanner()
//        {
//            if (!isBannerAdEnabled)
//                return;

//            if (isBannerAdLoaded)
//            {
//                MaxSdk.ShowBanner(BannerAdUnitId);
//            }
//            else
//            {
//            }
//        }

//        public void HideBanner()
//        {
//            MaxSdk.HideBanner(BannerAdUnitId);
//        }

//        public bool IsBannerAdLoaded()
//        {
//            return isBannerAdLoaded;
//        }

//        private void OnBannerAdLoadedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
//        {
//            isBannerAdLoaded = true;
//        }

//        private void OnBannerAdFailedEvent(string adUnitId, MaxSdkBase.ErrorInfo errorInfo)
//        {
//            isBannerAdLoaded = false;
//            //Debug.LogError($"Banner ad failed to load with error code: {errorInfo.Code}");
//            MaxSdk.CreateBanner(BannerAdUnitId, MaxSdkBase.BannerPosition.BottomCenter);
//        }

//        private void OnBannerAdClickedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
//        {
//        }

//        private void OnBannerAdRevenuePaidEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
//        {
//            //TrackAdRevenue(adInfo);
//        }

//        #endregion

//        //private void TrackAdRevenue(MaxSdkBase.AdInfo adInfo)
//        //{
//        //    AdjustAdRevenue adjustAdRevenue = new AdjustAdRevenue("applovin_max_sdk")
//        //    {
//        //        AdRevenueNetwork = adInfo.NetworkName,
//        //        AdRevenueUnit = adInfo.AdUnitIdentifier,
//        //        AdRevenuePlacement = adInfo.Placement,
//        //    };

//        //    adjustAdRevenue.SetRevenue(adInfo.Revenue, "USD");

//        //    Adjust.TrackAdRevenue(adjustAdRevenue);
//        //}
//    }
//}