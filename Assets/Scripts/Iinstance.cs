using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Iinstance : MonoBehaviour
{
    public static Iinstance instance;
    public List<GameObject> ObjectPool = new();
    public int BestScore;
    public int Conins;
    private void Awake()
    {
        Application.targetFrameRate = 60;
        Conins = PlayerPrefs.GetInt("Stars", 0);
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
}
