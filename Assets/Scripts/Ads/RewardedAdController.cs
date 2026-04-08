using UnityEngine;
using GoogleMobileAds.Api;
using System;

public class RewardedAdController : MonoBehaviour
{
#if UNITY_ANDROID
    private string _adUnitId = "ca-app-pub-3940256099942544/5224354917";
#elif UNITY_IPHONE
    private string _adUnitId = "ca-app-pub-3940256099942544/1712485313";
#else
    private string _adUnitId = "unused";
#endif

    private RewardedAd _rewardedAd;
    private Action _onRewarded;

    public static RewardedAdController Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        LoadRewardedAd();
    }

    // ================= LOAD ADS =================
    public void LoadRewardedAd()
    {
        if (_adUnitId == "unused")
        {
            Debug.LogWarning("Unsupported platform.");
            return;
        }

        Debug.Log("Loading rewarded ad...");

        AdRequest request = new AdRequest();

        RewardedAd.Load(_adUnitId, request, (RewardedAd ad, LoadAdError error) =>
        {
            if (error != null || ad == null)
            {
                Debug.LogError("Load failed: " + error);
                Invoke(nameof(LoadRewardedAd), 2f);
                return;
            }

            Debug.Log("Rewarded ad loaded.");
            _rewardedAd = ad;

            RegisterEvents(_rewardedAd);
        });
    }

    // ================= SHOW ADS =================
    public void ShowRewardedAd(Action onReward)
    {
        if (_rewardedAd != null && _rewardedAd.CanShowAd())
        {
            Debug.Log("Showing rewarded ad...");

            _onRewarded = onReward;

            _rewardedAd.Show(reward =>
            {
                Debug.Log($"User earned reward: {reward.Amount} {reward.Type}");
            });

            _rewardedAd = null;
        }
        else
        {
            Debug.Log("Ad not ready → reload");
            LoadRewardedAd();
        }
    }

    // ================= EVENTS =================
    private void RegisterEvents(RewardedAd ad)
    {
        ad.OnAdFullScreenContentOpened += () =>
        {
            Debug.Log("Ads Opened → Pause audio");
            AudioListener.pause = true;
        };

        ad.OnAdFullScreenContentClosed += () =>
        {
            Debug.Log("Ads Closed → Resume audio");

            AudioListener.pause = false;

            if (_onRewarded != null)
            {
                try
                {
                    _onRewarded.Invoke();
                }
                catch (Exception e)
                {
                    Debug.LogError("Reward callback error: " + e);
                }

                _onRewarded = null;
            }

            LoadRewardedAd();
        };

        ad.OnAdFullScreenContentFailed += (AdError error) =>
        {
            Debug.LogError("Ad show failed: " + error);

            AudioListener.pause = false;
            _onRewarded = null;

            LoadRewardedAd();
        };
    }

    private void OnDestroy()
    {
        if (_rewardedAd != null)
        {
            _rewardedAd.Destroy();
        }
    }
}