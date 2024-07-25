using UnityEngine;
using TMPro;
using CustomEventBus;

public class ChooseControler : MonoBehaviour
{
    [SerializeField] private float _time = 30;
    [SerializeField] private TMP_Text _textOfTime;
    private int _timeForUnhid = 20;
    private void Update()
    {
        if (_time >= 0)
        {
            _time -= Time.deltaTime;
            _textOfTime.text = $"{(int)_time}";
        }
        if (_timeForUnhid >= _time)
        {
            EventBus.TimeToUpdateHidNumber.Invoke();
            _timeForUnhid -= 10;
        }
    }
}
