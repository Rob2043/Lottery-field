using System.Collections;
using System.Collections.Generic;
using CustomEventBus;
using JetBrains.Annotations;
using UnityEngine;

public class Iinstance : MonoBehaviour
{
    private const int AmountOfTask = 3;
    public static Iinstance instance;
    public int Coins;
    public int FreeSpins;
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
    private void SetCoins(int Amount) => Coins += Amount;
    private void SetFreeSpin(int Amount) => FreeSpins += Amount;
    private int GetCoins()
    {
        return Coins;
    }
    private void OnApplicationQuit()
    {
        PlayerPrefs.SetInt("Money", Coins);
        PlayerPrefs.SetInt("FreeSpin", FreeSpins);
        PlayerPrefs.Save();
    }
}
