using UnityEngine;
using CustomEventBus;
using TMPro;

public class InputControler : MonoBehaviour
{
    [SerializeField] private GameObject _chouseTicket;
    [SerializeField] private TMP_Text _ScoreOfTickets;
    [SerializeField] private AudioSource _clickSound;
    private int _AmountOfTicket = 2;

    private void Start()
    {
        if (EventBus.SituationWithCountOfGuessNumber.Invoke() == false)
            _AmountOfTicket = 1;
        _ScoreOfTickets.text = $"{_AmountOfTicket}";
    }
    private void Update()
    {
        if(Input.touchCount > 0)
            EventBus.ExitLevelPanel.Invoke();
    }
    public void ChoseButton(int numberOfButton)
    {
        _clickSound.Play();
        if (_AmountOfTicket != 0)
        {
            Instantiate(_chouseTicket, EventBus.GetTransformForBillet.Invoke(numberOfButton));
            EventBus.ChouseNumber.Invoke(numberOfButton);
            _AmountOfTicket--;
            _ScoreOfTickets.text = $"{_AmountOfTicket}";
        }
    }
}
