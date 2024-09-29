using UnityEngine;
using TMPro;
using CustomEventBus;

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
    [SerializeField] private GameObject _rewardText;
    [SerializeField] private GameObject _winImage;
    [SerializeField] private ParticleSystem _confetti;
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
        if(PlayerPrefs.GetInt("TutorialCompletedPart2", 0) == 1)
        if (_time >= 0)
        {
            _time -= Time.deltaTime;
            _textOfTime.text = $"{(int)_time}";
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
            (bool wasWin, int value) =  EventBus.ReadyForCheck.Invoke();
            if (wasWin == true)
            {     
                int price = _winScore * value;
                EventBus.SetCoins(price);
                _resultText.text = "You Won!";
                _textOfMoney.text = $"{EventBus.GetCoins.Invoke()}";
                _textOfPriceMoney.text = $"{price}";
                _confetti.Play();
            }
            else
            {
                _winImage.SetActive(false); 
                _rewardText.SetActive(false);
                _resultText.text = "You Lose";
            }
                
            _endGamePanel.SetActive(true);
            _wasWin = true;
        }
    }
    private void IsChoose(int indexOfButton)
    {
        _wasChousing = true;
        EventBus.SetPlayersChouse.Invoke(indexOfButton);
    }  
}
