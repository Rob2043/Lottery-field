using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Iinstance : MonoBehaviour
{
    public static Iinstance instance;
    public int Conins;
    private void Awake()
    {
        Application.targetFrameRate = 60;
        Conins = PlayerPrefs.GetInt("Money", 0);
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
        Conins+=Amount;
    }
    private int GetCoins()
    {
        return Conins;
    }
    private void OnApplicationQuit()
    {
        PlayerPrefs.SetInt("Money",Conins);
    }
}
