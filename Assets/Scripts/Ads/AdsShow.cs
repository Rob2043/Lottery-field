using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CustomEventBus;

public class AdsShow : MonoBehaviour
{

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

    }
    private void RewardAds()
    {

    }
}
