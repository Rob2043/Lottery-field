using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CustomEventBus;

public class InputControler : MonoBehaviour
{
    [SerializeField] private GameObject _chouseTicket;
    private void ChoseButton(int numberOfButton)
    {
        Instantiate(_chouseTicket,EventBus.GetTransformForBillet.Invoke(numberOfButton));
        EventBus.ChouseNumber.Invoke(numberOfButton);
    }
}
