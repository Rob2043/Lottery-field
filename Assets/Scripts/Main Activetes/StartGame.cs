using System.Collections.Generic;
using TMPro;
using UnityEngine;
using CustomEventBus;

public class StartGame : MonoBehaviour
{
    [SerializeField] private TMP_Text[] _arrayTextNumbers = new TMP_Text[8];
    [SerializeField] private TMP_Text _textOfHidNumber;
    private Dictionary<int, int> RememberNumber = new();
    private Queue<int> RememberOfPeriod = new();
    private int[] RandomNumbers = new int[8];
    private int _choiseNumber = 0;
    private int _countOfNumbers;
    private int[] _guessNumnber;
    private int _guessDigit = 0;
    private int _hiddenNumber = 4;

    private void Awake()
    {
        EventBus.GetTransformForBillet = GetPositionForButton;
        EventBus.TimeToUpdateHidNumber = OnTime;
        EventBus.SetPlayersChouse = PlayersChouse;
        EventBus.ReadyForCheck = CheckWining;
        _countOfNumbers = Random.Range(6, 8);
        _guessNumnber = new int[_countOfNumbers];
        int _copyhidNumber = _hiddenNumber;
        for (int i = 0; i < _guessNumnber.Length; i++)
        {
            int _hidRandomDigit = Random.Range(0, 2);
            _guessNumnber[i] = Random.Range(0, 9);
            if (_guessDigit == 0)
                _guessDigit = _guessNumnber[i];
            else
                _guessDigit = _guessDigit * 10 + _guessNumnber[i];
            if (_hidRandomDigit == 0 && _copyhidNumber != 0)
            {
                RememberOfPeriod.Enqueue(i);
                RememberNumber.Add(i, _guessNumnber[i]);
                if (_guessNumnber[i] != 5)
                    _guessNumnber[i] = 5;
                else
                    _guessNumnber[i] = 4;
                _copyhidNumber--;
            }
        }
        int RandomPeriodNumber = Random.Range(0, 7);
        for (int i = 0; i < RandomNumbers.Length; i++)
        {
            int _randomNumber = RandomNumbers[i];
            if (i == RandomPeriodNumber)
                _randomNumber = _guessDigit;
            else
            {
                int _guessRandomDigit = 0;
                for (int j = 0; j < _countOfNumbers; j++)
                {
                    _guessRandomDigit = Random.Range(0, 9);
                    if (_randomNumber == 0)
                        _randomNumber = _guessRandomDigit;
                    else
                        _randomNumber = _randomNumber * 10 + _guessRandomDigit;
                }
            }
            _arrayTextNumbers[i].text = $"{_randomNumber}";
            RandomNumbers[i] = _randomNumber;
        }
        int periodNumber = RememberOfPeriod.Dequeue();
        DisclosureNumber(_guessNumnber, periodNumber, RememberNumber[periodNumber], false);
    }
    private void OnTime()
    {
        int periodNumber = RememberOfPeriod.Dequeue();
        Debug.Log(periodNumber);
        DisclosureNumber(_guessNumnber, periodNumber, RememberNumber[periodNumber], true);
    }

    private Transform GetPositionForButton(int index)
    {
        return _arrayTextNumbers[index].transform.parent.transform;
    }

    private bool CheckWining()
    {
        if(_choiseNumber == _guessDigit)
            return true;
        else
            return false;
    }

    private void PlayersChouse(int hisNumber)
    {
        _choiseNumber = RandomNumbers[hisNumber];
    }

    private void DisclosureNumber(int[] ArrayOfNumber, int keyNumber, int valeyOfKey, bool NeedOrNot)
    {
        string name = "";
        for (int i = 0; i < ArrayOfNumber.Length; i++)
        {
            int SomeNumber = ArrayOfNumber[i];
            if (keyNumber == i && NeedOrNot == true)
            {
                name = name + valeyOfKey;
                ArrayOfNumber[i] = valeyOfKey;
            }
            else
            {
                if (SomeNumber == 5 && RememberNumber.ContainsKey(i) && RememberNumber[i] != SomeNumber)
                    name = name + "*";
                else
                    name = name + SomeNumber;
            }
        }
        _textOfHidNumber.text = name;
    }
}
// private int ToNummber(int[] EnterArray)
// {
//     int _hidNumber = 0;
//     for (int i = 0; i < EnterArray.Length; i++)
//     {
//         if (_hidNumber == 0)
//             _hidNumber = EnterArray[i];
//         else
//             _hidNumber = _hidNumber * 10 + EnterArray[i];
//     }
//     return _hidNumber;
// }