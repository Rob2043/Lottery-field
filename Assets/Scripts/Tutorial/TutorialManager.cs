using System.Collections;
using CustomEventBus;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TutorialManager : MonoBehaviour
{
    const int numberForStartPart2 = 2;
    const int endForTutorial = 11;
    [SerializeField] private GameObject _tutorialPanel;
    [SerializeField] private Image[] _buttons;
    [SerializeField] private Image _spinbutton;
    [SerializeField] private GameObject _chosePanel;
    [SerializeField] private GameObject _taskPanel;
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
                OutButton();
            }
        }

    }
    public void StartTutotial()
    {
        _chosePanel.SetActive(false);
        _tutorialPanel.SetActive(true);
        OutButton();
    }
    public void OutButton()
    {
        if (!isCoroutineRunning)
            StartCoroutine(NextText());
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
            OutButton();
        }
        return result;
    }
    public IEnumerator NextText()
    {
        if (_countOfTutorials < numberForStartPart2 || PlayerPrefs.GetInt("TutorialCompletedPart2", 0) == 1)
        {
            isCoroutineRunning = true;
            _textForTutorial.text = "";
            _countOfTutorials++;
            switch (_countOfTutorials)
            {
                case 2:
                case 3:
                    _buttonForSwitchText.interactable = false;
                    break;
                case 8:
                    Color newColor;
                    if (ColorUtility.TryParseHtmlString(lightColor, out newColor))
                    {
                        for (int i = 0; i < _buttons.Length; i++)
                            _buttons[i].color = newColor;
                    }
                    break;
                case 9:
                    _taskPanel.SetActive(false);
                    for (int i = 0; i < _buttons.Length; i++)
                        _buttons[i].color = Color.white;
                    _buttonForSwitchText.interactable = false;
                    break;
                case 10:
                    Color newColor2;
                    if (ColorUtility.TryParseHtmlString(lightColor, out newColor2))
                        _spinbutton.color = newColor2;
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
            EventBus.ChangeBackground(false);
            PlayerPrefs.SetInt("TutorialCompleted", 1);
            _tutorialPanel.SetActive(false);
        }
        PlayerPrefs.Save();
        _writingTextSound.Stop();
        isCoroutineRunning = false;
    }
}
