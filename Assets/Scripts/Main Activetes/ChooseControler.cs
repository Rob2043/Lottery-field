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
    [SerializeField] private RectTransform _itemStar;
    [SerializeField] private GameObject _winPanel;
    [SerializeField] private GameObject _losePanel;
    [SerializeField] private GameObject _endGamePanel;
    [SerializeField] private GameObject _levelMessage;
    [SerializeField] private TMP_Text _levelMessageText;
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
                if(PlayerPrefs.GetInt("Ended",0) == 1)
                {
                    EventBus.ShowAds.Invoke();
                    PlayerPrefs.SetInt("Ended", 0);
                }
                else 
                    PlayerPrefs.SetInt("Ended", 1);
                EventBus.ChangeBackgroundInGame.Invoke(true);
                int level = Iinstance.instance.MyLevel;
                (bool wasWin, int value) = EventBus.ReadyForCheck.Invoke();
                if (wasWin == true)
                {
                    float percent = level * 0.25f + level;
                    if (level < 6)
                    {
                        PlayerPrefs.SetInt("Level", level++);
                        _levelMessageText.text = "Level up!";
                        Iinstance.instance.MyLevel = level;
                        EventBus.InfoLevel.Invoke(true);
                    }
                    int price = (int)(_winScore * value * percent);
                    EventBus.SetCoins.Invoke(price);
                    _resultText.text = "You Won!";
                    switch (price)
                    {
                        case < 1000:
                            _itemStar.anchoredPosition = new Vector2(_itemStar.anchoredPosition.x - 150f, _itemStar.anchoredPosition.y);
                            break;
                        case < 10000:
                            _itemStar.anchoredPosition = new Vector2(_itemStar.anchoredPosition.x - 50f, _itemStar.anchoredPosition.y);
                            break;
                    }
                    _textOfMoney.text = $"{EventBus.GetCoins.Invoke()}";
                    _textOfPriceMoney.text = $"{price}";
                    _confetti.Play();
                    _winPanel.SetActive(true);
                }
                else
                {
                    level = 1;
                    _levelMessageText.text = $"Level down!";
                    _resultText.text = "You Lose";
                    EventBus.InfoLevel.Invoke(false);
                    Iinstance.instance.MyLevel = level;
                    _winPanel.SetActive(false);
                    _losePanel.SetActive(true);
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
