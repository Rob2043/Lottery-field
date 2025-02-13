using UnityEngine;
using CustomEventBus;
using CrazyGames;

public class AdsShow : MonoBehaviour
{

    void Awake()
    {
        if (CrazySDK.IsAvailable)
        {
            CrazySDK.Init(() => { });
        }
    }
    private void OnEnable()
    {
        EventBus.ShowAds += ShowAds;
        EventBus.RewardAds += RewardAds;
    }
    private void OnDisable()
    {
        EventBus.ShowAds -= ShowAds;
        EventBus.RewardAds -= RewardAds;
    }
    private void ShowAds()
    {
        CrazySDK.Ad.RequestAd(
            CrazyAdType.Midgame,
            () =>
            {
                Debug.Log("Midgame ad started");
            },
            (error) =>
            {
                Debug.Log("Midgame ad error: " + error);
            },
            () =>
            {
                Debug.Log("Midgame ad finished");
            }
        );
    }
    private void RewardAds()
    {
        CrazySDK.Ad.RequestAd(
            CrazyAdType.Rewarded,
            () =>
            {
                Debug.Log("Rewarded ad started");
            },
            (error) =>
            {
                Debug.Log("Rewarded ad error: " + error);
            },
            () =>
            {
                Debug.Log("Rewarded ad finished, reward the player here");
                EventBus.SetCoins.Invoke(500);
                EventBus.UpdateMoney.Invoke();
            }
        );
    }
}
