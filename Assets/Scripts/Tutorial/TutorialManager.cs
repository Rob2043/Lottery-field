using System.Collections;
using CustomEventBus;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TutorialManager : MonoBehaviour
{
    const int numberForStartPart2 = 2;
    const int endForTutorial = 12;
    [SerializeField] private GameObject _tutorialPanel;
    [SerializeField] private Image[] _buttons;
    [SerializeField] private Image _spinbutton;
    [SerializeField] private Image _levelIcon;
    [SerializeField] private Image[] _playButtons = new Image[3];
    [SerializeField] private TMP_Text[] _playTexts = new TMP_Text[3];
    [SerializeField] private GameObject _chosePanel;
    [SerializeField] private GameObject _taskPanel;
    [SerializeField] private GameObject _spinPanel;
    [SerializeField] private string[] _tutorialsTexts;
    [SerializeField] private TMP_Text _textForTutorial;
    [SerializeField] private AudioSource _writingTextSound;
    [SerializeField] private Button _buttonForSwitchText;
    private bool isCoroutineRunning = false;
    int _countOfTutorials = 0;
    string lightColor = "#FF0000";

    private void Start()
    {
        if (PlayerPrefs.GetInt("TutorialCompleted", 0) == 0)
        {
            EventBus.CanPlay = CheckPlaing;
            if (PlayerPrefs.GetInt("TutorialCompletedPart1", 0) == 0)
                _chosePanel.SetActive(true);
            else if (PlayerPrefs.GetInt("TutorialCompletedPart2", 0) == 1)
            {
                _tutorialPanel.SetActive(true);
                _countOfTutorials = numberForStartPart2;
                OutButton(false);
            }
        }

    }
    public void StartTutotial()
    {
        _chosePanel.SetActive(false);
        _tutorialPanel.SetActive(true);
        OutButton(false);
    }
    public void OutButton(bool result)
    {
        if (!isCoroutineRunning)
            StartCoroutine(NextText(result));
    }
    private bool CheckPlaing()
    {
        bool result = false;
        result = _countOfTutorials switch
        {
            2 => true,
            3 => true,
            9 => true,
            10 => true,
            _ => false,
        };
        if (result == true)
        {
            _buttonForSwitchText.interactable = true;
            _countOfTutorials++;
            OutButton(result);
        }
        return result;
    }
    private void ChangeColor(int Number)
    {
        if (Number == 2)
        {
            _playButtons[Number].color = Color.white;
            _playButtons[Number + 1].color = Color.white;

        }
        else
        {
            _playButtons[Number].color = Color.white;
            _playTexts[Number].color = Color.white;
        }
    }
    public IEnumerator NextText(bool WasClickAnotherButton)
    {
        if (_countOfTutorials < numberForStartPart2 || PlayerPrefs.GetInt("TutorialCompletedPart2", 0) == 1)
        {
            isCoroutineRunning = true;
            _textForTutorial.text = "";
            if(WasClickAnotherButton == false)
                _countOfTutorials++;
            switch (_countOfTutorials)
            {
                case 2:
                    _buttonForSwitchText.interactable = false;
                    ChangeColor(0);
                    break;
                case 3:
                    ChangeColor(1);
                    _buttonForSwitchText.interactable = false;
                    break;
                case 8:
                    for (int i = 0; i < _buttons.Length; i++)
                        _buttons[i].color = Color.blue;
                    break;
                case 9:
                    ChangeColor(2);
                    _taskPanel.SetActive(false);
                    for (int i = 0; i < _buttons.Length; i++)
                        _buttons[i].color = Color.white;
                    _buttonForSwitchText.interactable = false;
                    break;
                case 10:
                    Color newColor2;
                    if (ColorUtility.TryParseHtmlString(lightColor, out newColor2))
                        _spinbutton.color = newColor2;
                    _buttonForSwitchText.interactable = false;
                    _tutorialPanel.SetActive(false);
                    break;
                case 11:
                    _tutorialPanel.SetActive(true);
                    _spinPanel.SetActive(false);
                    EventBus.ChangeBackground.Invoke(true,_levelIcon);
                break;
            }
            _writingTextSound.Play();
            for (int i = 0; i < _tutorialsTexts[_countOfTutorials].Length; i++)
            {
                _textForTutorial.text += _tutorialsTexts[_countOfTutorials][i];
                yield return new WaitForSecondsRealtime(0.02f);
            }
        }
        if (_countOfTutorials == endForTutorial)
        {
            EventBus.ChangeBackground(false,null);
            PlayerPrefs.SetInt("TutorialCompleted", 1);
            _tutorialPanel.SetActive(false);
        }
        PlayerPrefs.Save();
        _writingTextSound.Stop();
        isCoroutineRunning = false;
    }
}
