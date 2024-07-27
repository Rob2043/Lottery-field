using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CustomEventBus;

public class InputControler : MonoBehaviour
{
    [SerializeField] private GameObject _chouseTicket;
    public void ChoseButton(int numberOfButton)
    {
        if (PlayerPrefs.GetInt("ChooseCount", 0) == 0)
        {
            Instantiate(_chouseTicket, EventBus.GetTransformForBillet.Invoke(numberOfButton));
            EventBus.ChouseNumber.Invoke(numberOfButton);
            PlayerPrefs.SetInt("ChooseCount", 1);
        }

    }
}
