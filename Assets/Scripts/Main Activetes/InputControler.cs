using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CustomEventBus;

public class InputControler : MonoBehaviour
{
    [SerializeField] private GameObject _chouseTicket;
    [SerializeField] private TMP_Text _ScoreOfTickets;
    private int _AmountOfTicket = 2;

    private void Start()
    {
        _ScoreOfTickets.text = $"{_AmountOfTicket}";
    }
    public void ChoseButton(int numberOfButton)
    {
        if(_AmountOfTicket != 0)
        { 
            Instantiate(_chouseTicket, EventBus.GetTransformForBillet.Invoke(numberOfButton));
            EventBus.ChouseNumber.Invoke(numberOfButton);
            _AmountOfTicket--;
            _ScoreOfTickets.text = $"{_AmountOfTicket}";
        }
    }
}
