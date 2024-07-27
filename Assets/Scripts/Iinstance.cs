using System.Collections;
using System.Collections.Generic;
using CustomEventBus;
using UnityEngine;

public class Iinstance : MonoBehaviour
{
    public static Iinstance instance;
    public int Coins;
    private void Awake()
    {
        EventBus.SetCoins = SetCoins;
        EventBus.GetCoins = GetCoins;
        Application.targetFrameRate = 60;
        Coins = PlayerPrefs.GetInt("Money", 0);
        PlayerPrefs.SetInt("ChooseCount", 0);
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }
    private void SetCoins(int Amount)
    {
        Coins += Amount;
    }
    private int GetCoins()
    {
        return Coins;
    }
    private void OnApplicationQuit()
    {
        PlayerPrefs.SetInt("Money", Coins);
    }
}
