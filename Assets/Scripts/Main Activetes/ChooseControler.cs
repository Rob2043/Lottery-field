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
    [Header("Others")]
    [SerializeField] private GameObject _endGamePanel;
    [SerializeField] private GameObject _levelMessage;
    [SerializeField] private TMP_Text _levelMessageText;
    [SerializeField] private GameObject _rewardText;
    [SerializeField] private GameObject _winImage;
    [SerializeField] private ParticleSystem _confetti;
    [SerializeField] private Slider timerSlider;

    private int _timeForUnhid = 20;
    private int _winScore = 1000;
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
                if (_timeForUnhid >= _time)
                {
                    if (_wasChousing == false)
                        _winScore /= 2;
                    EventBus.TimeToUpdateHidNumber.Invoke();
                    _timeForUnhid -= 10;
                }
            }
            if (_time <= 0 && _wasWin == false)
            {
                int level = Iinstance.instance.MyLevel;
                (bool wasWin, int value) = EventBus.ReadyForCheck.Invoke();
                if (wasWin == true)
                {
                    PlayerPrefs.SetInt("Level", Iinstance.instance.MyLevel++);
                    _levelMessageText.text = $"You upgraded your level to{level}";
                    float percent = level * 0.25f + level;
                    int price = (int)(_winScore * value * percent);
                    EventBus.SetCoins(price);
                    _resultText.text = "You Won!";
                    _textOfMoney.text = $"{EventBus.GetCoins.Invoke()}";
                    _textOfPriceMoney.text = $"{price}";
                    _confetti.Play();
                }
                else
                {
                    Iinstance.instance.MyLevel = 1;
                    _levelMessageText.text = $"You downgraded your level to {level}";
                    _winImage.SetActive(false);
                    _rewardText.SetActive(false);
                    _resultText.text = "You Lose";
                }
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
