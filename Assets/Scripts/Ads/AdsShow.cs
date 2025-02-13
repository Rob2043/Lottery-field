using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CustomEventBus;

public class AdsShow : MonoBehaviour
{
    private int AdID = 0;
    void Start()
    {

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
        EventBus.YandexShowAds?.Invoke();
    }
    private void RewardAds()
    {
        EventBus.YandexRewardAds?.Invoke(AdID);
    }
}
