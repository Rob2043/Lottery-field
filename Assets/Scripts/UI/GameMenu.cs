using System.Collections;
using CustomEventBus;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class LocalMenu : MonoBehaviour
{
    private const string SoundPreference = "isSoundOn";
    private const float VolumeOn = 0f;
    private const float VolumeOff = -80f;
    [SerializeField] private GameObject exitPanel;
    [SerializeField] private GameObject gameDisplayPanel;
    [SerializeField] private GameObject endGamePanel;
    [SerializeField] private AudioSource[] audioSources;
    [SerializeField] private AudioMixer mainAudioMixer;
    [SerializeField] private Button soundToggleButton;
    [SerializeField] private Sprite soundOnSprite;
    [SerializeField] private Sprite soundOffSprite;
    [SerializeField] private TMP_Text _moneyText;

    private bool isSoundActive = false;

    public float totalTime = 30f;

    private void Start()
    {
        InitializeAudioSettings();
        _moneyText.text = $"{EventBus.GetCoins.Invoke()}";
        Time.timeScale = 1f;
    }
    public void ToggleSound()
    {
        isSoundActive = !isSoundActive;
        audioSources[1].Play();
        foreach (var audio in audioSources)
        {
            audio.enabled = isSoundActive;
        }
        mainAudioMixer.SetFloat("MasterVolume", isSoundActive ? VolumeOn: VolumeOff);
        PlayerPrefs.SetInt(SoundPreference, isSoundActive ? 1 : 0);
        PlayerPrefs.Save();
        soundToggleButton.image.sprite = isSoundActive ? soundOnSprite : soundOffSprite;
    }
    public void OpenSettings()
    {
        audioSources[1].Play();
        exitPanel.SetActive(true);
    }

    public void CloseSettings()
    {
        audioSources[1].Play();
        exitPanel.SetActive(false);
    }
    private void InitializeAudioSettings()
    {
        int soundStatus = PlayerPrefs.GetInt(SoundPreference, 1);
        if (soundStatus == 1)
        {
            mainAudioMixer.SetFloat("MasterVolume", VolumeOn);
            audioSources[0].Play();
            audioSources[1].Play();
        }
        else
        {
            mainAudioMixer.SetFloat("MasterVolume", VolumeOff);
        }
    }

    public void ReturnToMainMenu()
    {
        audioSources[1].Play();
        SceneManager.LoadScene("MainMenu");
    }

    public void Again()
    {
        audioSources[1].Play();
        SceneManager.LoadScene("GameScene");
    }
    public void ShowExitPanel()
    {
        Time.timeScale = 0f;
        audioSources[1].Play();
        gameDisplayPanel.SetActive(false);
        exitPanel.SetActive(true);
    }

    public void HideExitPanel()
    {
        Time.timeScale = 1f;
        audioSources[1].Play();
        exitPanel.SetActive(false);
        if (!endGamePanel.activeSelf)
        {
            gameDisplayPanel.SetActive(true);
        }
    }
}
