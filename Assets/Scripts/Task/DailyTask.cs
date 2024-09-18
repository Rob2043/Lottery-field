using System;
using System.Collections;
using CustomEventBus;
using UnityEngine;

public class DailyTask : MonoBehaviour
{
    [SerializeField] private GameObject _dailyPanel;
    private const string LastUpdateTimeKey = "LastUpdateTime";
    void Start()
    {
        string lastUpdateTime = PlayerPrefs.GetString(LastUpdateTimeKey, DateTime.MinValue.ToString());
        DateTime lastUpdateDateTime = DateTime.Parse(lastUpdateTime);
        if ((DateTime.Now - lastUpdateDateTime).TotalSeconds >= 86400)
        {
            _dailyPanel.SetActive(true);
        }
    }
    private void OnApplicationQuit()
    {
        PlayerPrefs.SetString(LastUpdateTimeKey, DateTime.Now.ToString());
        PlayerPrefs.Save();
    }
    public void GiveMoney()
    {
        EventBus.SetCoins.Invoke(300);
        EventBus.UpdateMoney.Invoke();
    }
}
