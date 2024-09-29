using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using CustomEventBus;

public class GameTutorial : MonoBehaviour
{
    const int numberForStartPart2 = 6;
    [SerializeField] private GameObject _tutorialPanel;
    [SerializeField] private string[] _tutorialsTexts;
    [SerializeField] private TMP_Text _textForTutorial;
    [SerializeField] private AudioSource _writingTextSound;
    int _countOfTutorials = 0;
    private void Start()
    {
        if (PlayerPrefs.GetInt("TutorialCompletedPart1", 1) == 1)
            _tutorialPanel.SetActive(true);
    }
    public IEnumerator NextText()
    {
        if (_countOfTutorials < numberForStartPart2 )
        {
            _textForTutorial.text = "";
            _countOfTutorials++;
            _writingTextSound.Play();
            for (int i = 0; i < _tutorialsTexts[_countOfTutorials].Length; i++)
            {
                _textForTutorial.text += _tutorialsTexts[_countOfTutorials][i];
                yield return new WaitForSecondsRealtime(0.02f);
            }
        }
        else if (_countOfTutorials == numberForStartPart2)
            PlayerPrefs.SetInt("TutorialCompletedPart2", 1);
        _writingTextSound.Stop();
    }
}
