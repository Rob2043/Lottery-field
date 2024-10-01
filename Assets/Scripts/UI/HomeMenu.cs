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
    [SerializeField] private TMP_Text _secondStarsText;
    [SerializeField] private TMP_Text _thirdStarsText;
    [SerializeField] private GameObject settingsPanel;
    [SerializeField] private GameObject ErrorPanel;
    [SerializeField] private GameObject SpinPanel;
    [SerializeField] private GameObject TaskPanel;
    [SerializeField] private GameObject TutorialPanel;
    [Header("Audio Settings")]
    [SerializeField] private AudioSource[] audioClips;
    [SerializeField] private Button soundToggleButton;
    [SerializeField] private Sprite soundOnSprite;
    [SerializeField] private Sprite soundOffSprite;
    [SerializeField] private AudioMixer masterAudioMixer;


    private bool isSoundActive;
    private int MinmumCostOfGame = 100;

    private void Start()
    {
        EventBus.UpdateMoney = UpdaeteMoney;
        UpdaeteMoney();
        Time.timeScale = 1f;
        InitializeAudioSettings();
    }
    private void UpdaeteMoney()
    {
        starsText.text = $"{Iinstance.instance.Coins}";
        _secondStarsText.text = $"{Iinstance.instance.Coins}";
        _thirdStarsText.text = $"{Iinstance.instance.Coins}";
    }
    private void InitializeAudioSettings()
    {
        isSoundActive = PlayerPrefs.GetInt("isSoundOn", 0) == 1;
        audioClips[1].Play();
        foreach (var audio in audioClips)
        {
            audio.enabled = isSoundActive;
        }
        soundToggleButton.image.sprite = isSoundActive ? soundOnSprite : soundOffSprite;
    }

    public void StartGame()
    {
        if (PlayerPrefs.GetInt("TutorialCompleted", 0) == 0)
        {
            if (EventBus.CanPlay.Invoke() == true)
            {
                audioClips[1].Play();
                if (PlayerPrefs.GetInt("TutorialCompletedPart1", 0) == 0)
                {
                    PlayerPrefs.SetInt("TutorialCompletedPart1", 1);
                    TutorialPanel.SetActive(false);
                }
                EventBus.LodingScene.Invoke("GameScene");
            }
        }
        else
        {
            if (MinmumCostOfGame <= EventBus.GetCoins.Invoke())
            {
                EventBus.SetCoins(-MinmumCostOfGame);
                EventBus.LodingScene.Invoke("GameScene");
            }
            else
                ErrorPanel.SetActive(true);
        }
        PlayerPrefs.Save();
    }
    public void OpenTask()
    {
        if (PlayerPrefs.GetInt("TutorialCompleted", 0) == 0)
        {
            if (EventBus.CanPlay.Invoke() == true)
            {
                TaskPanel.SetActive(true);
            }

        }
        else
            TaskPanel.SetActive(true);
    }
    public void OpenSettings()
    {
        audioClips[1].Play();
        settingsPanel.SetActive(true);
    }

    public void OpenSpin()
    {
        if (PlayerPrefs.GetInt("TutorialCompleted", 0) == 0)
        {
            if (EventBus.CanPlay.Invoke() == true)
            {
                SpinPanel.SetActive(true);
            }
        }
        else
            SpinPanel.SetActive(true);
    }
    public void GiveMoney()
    {
        SpinPanel.SetActive(true);
        EventBus.FreeSpin.Invoke(true);
        ErrorPanel.SetActive(false);
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
