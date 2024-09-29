using System.Collections;
using CustomEventBus;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TutorialManager : MonoBehaviour
{
    const int numberForStartPart2 = 1;
    [SerializeField] private GameObject _tutorialPanel;
    [SerializeField] private Image[] _buttons;
    [SerializeField] private GameObject _chosePanel;
    [SerializeField] private GameObject _taskPanel;
    [SerializeField] private string[] _tutorialsTexts;
    [SerializeField] private TMP_Text _textForTutorial;
    [SerializeField] private AudioSource _writingTextSound;
    [SerializeField] private Button _buttonForSwitchText;

    int _countOfTutorials = 0;
    string lightColor = "FFAAAA";
    private void Start()
    {
        if (PlayerPrefs.GetInt("TutorialCompleted", 0) == 0)
        {
            EventBus.CanPlay = CheckPlaing;
            EventBus.NextTextForTutorial = OutButton;
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
    private void OutButton() => StartCoroutine(NextText());
    private bool CheckPlaing()
    {
        bool result = false;
        result = _countOfTutorials switch
        {
            2 => true,
            3 => true,
            8 => true,
            _ => false,
        };
        if (result == true)
        {
            _buttonForSwitchText.interactable = true;
            _countOfTutorials++;
        }
        return result;
    }
    public IEnumerator NextText()
    {
        if (_countOfTutorials < numberForStartPart2 || PlayerPrefs.GetInt("TutorialCompletedPart2", 1) == 1)
        {
            _textForTutorial.text = "";
            _countOfTutorials++;
            switch (_countOfTutorials)
            {
                case 2:
                    _buttonForSwitchText.interactable = false;
                    break;
                case 7:
                    Color newColor;
                    if (ColorUtility.TryParseHtmlString(lightColor, out newColor))
                    {
                        for (int i = 0; i < _buttons.Length; i++)
                            _buttons[i].color = newColor;
                    }
                    break;
                case 8:
                    _taskPanel.SetActive(false);
                    for (int i = 0; i < _buttons.Length; i++)
                        _buttons[i].color = Color.white;
                    _buttonForSwitchText.interactable = false;
                    break;
            }
            _writingTextSound.Play();
            for (int i = 0; i < _tutorialsTexts[_countOfTutorials].Length; i++)
            {
                _textForTutorial.text += _tutorialsTexts[_countOfTutorials][i];
                yield return new WaitForSecondsRealtime(0.02f);
            }
        }
        _writingTextSound.Stop();
    }
}
