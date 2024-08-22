using System;
using CustomEventBus;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class HomeMenu : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private TMP_Text starsText;
    [SerializeField] private GameObject settingsPanel;
    [SerializeField] private GameObject ErrorPanel;
    [Header("Audio Settings")]
    [SerializeField] private AudioSource[] audioClips;
    [SerializeField] private Button soundToggleButton;
    [SerializeField] private Sprite soundOnSprite;
    [SerializeField] private Sprite soundOffSprite;
    [SerializeField] private AudioMixer masterAudioMixer;

    private bool isSoundActive;
    private int MinmumCostOfGame = 1000;

    private void Start()
    {
        InitializeMenu();
        InitializeAudioSettings();
    }

    private void InitializeMenu()
    {
        starsText.text = $"{Iinstance.instance.Coins}";
        Time.timeScale = 1f;
    }

    private void InitializeAudioSettings()
    {
        isSoundActive = PlayerPrefs.GetInt("isSoundOn", 1) == 1;
        audioClips[1].Play();
        foreach (var audio in audioClips)
        {
            audio.enabled = isSoundActive;
        }
        soundToggleButton.image.sprite = isSoundActive ? soundOnSprite : soundOffSprite;
    }

    public void StartGame()
    {
        audioClips[1].Play();
        if (MinmumCostOfGame <= EventBus.GetCoins.Invoke())
        {
            EventBus.SetCoins(-MinmumCostOfGame);
            EventBus.LodingScene.Invoke("GameScene");
        }
        else
            ErrorPanel.SetActive(true);
    }

    public void OpenSettings()
    {
        audioClips[1].Play();
        settingsPanel.SetActive(true);
    }

    public void ToggleSound()
    {
        isSoundActive = !isSoundActive;
        audioClips[1].Play();
        foreach (var audio in audioClips)
        {
            audio.enabled = isSoundActive;
        }
        masterAudioMixer.SetFloat("MasterVolume", isSoundActive ? 0f : -80f);
        PlayerPrefs.SetInt("isSoundOn", isSoundActive ? 1 : 0);
        PlayerPrefs.Save();
        soundToggleButton.image.sprite = isSoundActive ? soundOnSprite : soundOffSprite;
    }

    public void CloseSettings()
    {
        audioClips[1].Play();
        settingsPanel.SetActive(false);
    }

    public void ExitGame()
    {
        audioClips[1].Play();
#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
