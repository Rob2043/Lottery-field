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
    [SerializeField] private TMP_Text _levelText;
    [SerializeField] private TMP_Text _secondStarsText;
    [SerializeField] private TMP_Text _thirdStarsText;
    [SerializeField] private GameObject settingsPanel;
    [SerializeField] private GameObject ErrorPanel;
    [SerializeField] private GameObject SpinPanel;
    [SerializeField] private GameObject TaskPanel;
    [SerializeField] private GameObject TutorialPanel;
    [SerializeField] private Image[] allImages = new Image[3];
    [SerializeField] private Image mainBackGroundImage;
    [SerializeField] private TMP_Text[] _textOfButtons = new TMP_Text[2];
    [SerializeField] private TMP_Text _textOfFreeSpin;
    [SerializeField] private GameObject _chosePanel;

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
        _levelText.text = $"{Iinstance.instance.MyLevel}";
        EventBus.ChangeBackground = ChangeColor;
        if (PlayerPrefs.GetInt("TutorialCompleted", 0) == 0)
            ChangeColor(true);
        else
            ChangeColor(false);
        EventBus.UpdateMoney = UpdaeteMoney;
        UpdaeteMoney();
        Time.timeScale = 1f;
        InitializeAudioSettings();
    }
    public void ExitFromTheSpinPanel()
    {
        audioClips[1].Play();
        SpinPanel.SetActive(false);
        ChangeColor(false);
    }  

    private void ChangeColor(bool wasChangingColor)
    {
        if (wasChangingColor == true)
        {
            for (int i = 0; i < allImages.Length; i++)
            {
                allImages[i].color = Color.gray;
                if (i < 3)
                    _textOfButtons[i].color = Color.gray;
            }
            mainBackGroundImage.color = Color.gray;
        }
        else
        {
            for (int i = 0; i < allImages.Length; i++)
            {
                allImages[i].color = Color.white;
                if (i < 3)
                    _textOfButtons[i].color = Color.white;
            }
            mainBackGroundImage.color = Color.white;
        }
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
    public void CloseTutorial()
    {
        audioClips[1].Play();
        _chosePanel.SetActive(false);
        ChangeColor(false);
        PlayerPrefs.SetInt("TutorialCompleted", 1);
        PlayerPrefs.Save();
    }
    public void StartGame()
    {
        if (PlayerPrefs.GetInt("TutorialCompleted", 0) == 0)
        {
            ChangeColor(true);
            if (EventBus.CanPlay.Invoke() == true)
            {
                audioClips[1].Play();
                if (PlayerPrefs.GetInt("TutorialCompletedPart1", 0) == 0)
                {
                    ChangeColor(false);
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
            {
                ChangeColor(true);
                ErrorPanel.SetActive(true);
            }
        }
        PlayerPrefs.Save();
    }
    public void OpenTask()
    {
        ChangeColor(true);
        if (PlayerPrefs.GetInt("TutorialCompleted", 0) == 0)
        {
            if (EventBus.CanPlay.Invoke() == true)
                TaskPanel.SetActive(true);
        }
        else
            TaskPanel.SetActive(true);
    }
    public void CloseTask()
    {
        ChangeColor(false);
        TaskPanel.SetActive(false);
        audioClips[1].Play();
    }
    public void OpenSettings()
    {
        ChangeColor(true);
        audioClips[1].Play();
        settingsPanel.SetActive(true);
    }

    public void OpenSpin()
    {
        _textOfFreeSpin.text = $"{Iinstance.instance.FreeSpins}";
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
        ChangeColor(false);
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
