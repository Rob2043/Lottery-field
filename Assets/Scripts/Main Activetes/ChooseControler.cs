using UnityEngine;
using TMPro;
using CustomEventBus;
using UnityEngine.UI;


public class ChooseControler : MonoBehaviour
{
    [SerializeField] private float _time = 30;
    [Header("Texts")]
    [SerializeField] private TMP_Text _textOfTime;
    [SerializeField] private TMP_Text _textOfMoney;
    [SerializeField] private TMP_Text _textOfPriceMoney;
    [SerializeField] private TMP_Text _resultText;
    [SerializeField] private TMP_Text _infoLevelText;
    [Header("Others")]
    [SerializeField] private GameObject _endGamePanel;
    [SerializeField] private GameObject _levelMessage;
    [SerializeField] private TMP_Text _levelMessageText;
    [SerializeField] private GameObject _winPanel;
    [SerializeField] private GameObject _losePanel;
    [SerializeField] private ParticleSystem _confetti;
    [SerializeField] private Slider timerSlider;

    private int _timeForUnhid = 25;
    private int _winScore = 2000;
    private bool _wasChousing = false;
    private bool _wasWin = false;

    private void Start()
    {
        EventBus.ChouseNumber = IsChoose;
    }
    private void Update()
    {
        if (PlayerPrefs.GetInt("TutorialCompletedPart2", 0) == 1)
        {
            if (_time >= 0)
            {
                _time -= Time.deltaTime;
                _textOfTime.text = $"{(int)_time}";
                timerSlider.value = _time;
                if (_timeForUnhid >= _time && _time >= 5)
                {
                    if (_wasChousing == false)
                        _winScore /= 2;
                    EventBus.TimeToUpdateHidNumber.Invoke();
                    _timeForUnhid -= 5;
                }
            }
            if (_time <= 0 && _wasWin == false)
            {
                int level = Iinstance.instance.MyLevel;
                (bool wasWin, int value) = EventBus.ReadyForCheck.Invoke();
                if (wasWin == true)
                {
                    float percent = level * 0.25f + level;
                    if(level < 6)
                    {
                        PlayerPrefs.SetInt("Level", level++);
                        _levelMessageText.text = "You upgraded your level!";
                    }
                    int price = (int)(_winScore * value * percent);
                    EventBus.SetCoins.Invoke(price);
                    _winPanel.SetActive(true);
                    _resultText.text = "You Won!";
                    _textOfMoney.text = $"{EventBus.GetCoins.Invoke()}";
                    _textOfPriceMoney.text = $"{price}";
                    _confetti.Play();
                }
                else
                {
                    level = 1;
                    _levelMessageText.text = $"You downgraded your level!";
                    _losePanel.SetActive(true);
                    _resultText.text = "You Lose";
                }
                _infoLevelText.text = $"{level}";
                Iinstance.instance.MyLevel = level;
                EventBus.ChangeBackgroundInGame.Invoke(true);
                _levelMessage.SetActive(true);
                _endGamePanel.SetActive(true);
                _wasWin = true;
            }
        }

    }
    private void IsChoose(int indexOfButton)
    {
        _wasChousing = true;
        EventBus.SetPlayersChouse.Invoke(indexOfButton);
    }
}
