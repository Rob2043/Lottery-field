using System;
using System.Collections;
using UnityEngine;

public class DailyTask : MonoBehaviour
{
    private const string LastUpdateTimeKey = "LastUpdateTime";
    void Start()
    {
        string lastUpdateTime = PlayerPrefs.GetString(LastUpdateTimeKey, DateTime.MinValue.ToString());
        DateTime lastUpdateDateTime = DateTime.Parse(lastUpdateTime);
        if ((DateTime.Now - lastUpdateDateTime).TotalSeconds >= 86400)
        {

        }
    }
    private void OnApplicationQuit()
    {
        PlayerPrefs.SetString(LastUpdateTimeKey, DateTime.Now.ToString());
        PlayerPrefs.Save();
    }

}
