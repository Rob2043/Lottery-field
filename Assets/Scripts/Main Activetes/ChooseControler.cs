using UnityEngine;
using TMPro;
using CustomEventBus;

public class ChooseControler : MonoBehaviour
{
    [SerializeField] private float _time = 30;
    [SerializeField] private TMP_Text _textOfTime;
    [SerializeField] private GameObject _endGamePanel;
    [SerializeField] private TMP_Text _resultText;
    private int _timeForUnhid = 20;
    private int _winScore = 10000;
    private bool _wasChousing = false;
    private bool _wasWin = false;

    private void Start()
    {
        EventBus.ChouseNumber = IsChoose;
    }
    private void Update()
    {
        if (_time >= 0)
        {
            _time -= Time.deltaTime;
            _textOfTime.text = $"{(int)_time}";
            if (_timeForUnhid >= _time)
            {
                if (_wasChousing!)
                    _winScore /= 2;
                EventBus.TimeToUpdateHidNumber.Invoke();
                _timeForUnhid -= 10;
            }
        }
        if (_time <= 0 && _wasWin!)
        {
            if (EventBus.ReadyForCheck.Invoke() == true)
            {
                EventBus.SetCoins(_winScore);
                _resultText.text = "You Won";
            }
            else
                _resultText.text = "You Lose";
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
