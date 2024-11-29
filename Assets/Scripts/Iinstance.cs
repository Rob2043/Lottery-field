using System.Collections;
using System.Collections.Generic;
using CustomEventBus;
using UnityEngine;
using System;

public class Iinstance : MonoBehaviour
{
    private const int AmountOfTask = 3;
    public static Iinstance instance;
    public int Coins;
    public int FreeSpins;
    public int MyLevel;
    public Dictionary<string, float> TypesOfTask = new(AmountOfTask);
    public string[] ArrayOfNameTask = new string[AmountOfTask];
    private void Awake()
    {
        //PlayerPrefs.DeleteAll();
        Screen.orientation = ScreenOrientation.Portrait;
        Screen.autorotateToPortrait = true;
        Screen.autorotateToPortraitUpsideDown = true;
        Screen.autorotateToLandscapeLeft = false;
        Screen.autorotateToLandscapeRight = false;
        EventBus.SetCoins = SetCoins;
        EventBus.GetCoins = GetCoins;
        EventBus.AddFreeSpin = SetFreeSpin;
        Application.targetFrameRate = 60;
        Coins = PlayerPrefs.GetInt("Money", 1000);
        FreeSpins = PlayerPrefs.GetInt("FreeSpin", 0);
        MyLevel = PlayerPrefs.GetInt("Level", 1);
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
    private void SetCoins(int Amount) {
        Coins += Amount;
        Debug.Log(Coins);
    } 
    private void SetFreeSpin(int Amount) => FreeSpins += Amount;
    private int GetCoins()
    {
        return Coins;
    }
    void OnApplicationQuit()
    {
        PlayerPrefs.SetInt("Level",MyLevel);
        PlayerPrefs.SetInt("Money", Coins);
        PlayerPrefs.SetInt("FreeSpin", FreeSpins);
        PlayerPrefs.Save();
    }
}
