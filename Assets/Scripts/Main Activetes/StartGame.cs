using System.Collections.Generic;
using TMPro;
using UnityEngine;
using CustomEventBus;

public class StartGame : MonoBehaviour
{
    [SerializeField] private TMP_Text[] _arrayTextNumbers = new TMP_Text[16];
    [SerializeField] private TMP_Text _textOfFirstHidNumber;
    [SerializeField] private TMP_Text _textOfSecondHidNumber;
    private Dictionary<int, int> RememberFirstNumber = new();
    private Queue<int> RememberOfFirstPeriod = new();
    private int[] _FirstGuessNumber;
    private int _FirstGuessDigit = 0;
    private int _SecondGuessDigit = 0;
    private Dictionary<int, int> RememberSecondNumber = new();
    private Queue<int> RememberOfSecondPeriod = new();
    private int[] _SecondGuessNumber;
    private int[] RandomNumbers = new int[16];
    private int[] _choiseNumbers = new int[2];
    private int _countOfChouising = 0;
    private int _countOfNumbers;
    private int _hiddenNumber = 4;

    private void Awake()
    {
        EventBus.GetTransformForBillet = GetPositionForButton;
        EventBus.TimeToUpdateHidNumber = OnTime;
        EventBus.SetPlayersChouse = PlayersChouse;
        EventBus.ReadyForCheck = CheckWining;
        _countOfNumbers = Random.Range(6, 8);
        _FirstGuessNumber = new int[_countOfNumbers];
        _SecondGuessNumber = new int[_countOfNumbers];
        int RandomPeriodFirstNumber = Random.Range(0, _countOfNumbers);
        int RandomPeriodSecondNumber = Random.Range(0, _countOfNumbers);
        (_FirstGuessDigit, _FirstGuessNumber) = IninizializationNumber(_FirstGuessNumber, _FirstGuessDigit, RememberOfFirstPeriod, RememberFirstNumber);
        (_SecondGuessDigit, _SecondGuessNumber) = IninizializationNumber(_SecondGuessNumber, _SecondGuessDigit, RememberOfSecondPeriod, RememberSecondNumber);
        for (int i = 0; i < RandomNumbers.Length; i++)
        {
            int _randomNumber = RandomNumbers[i];
            if (i == RandomPeriodFirstNumber)
                _randomNumber = _FirstGuessDigit;
            else if (i == RandomPeriodSecondNumber)
                _randomNumber = _SecondGuessDigit;
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
        int firstPeriodNumber = RememberOfFirstPeriod.Dequeue();
        int secondPeriodNumber = RememberOfSecondPeriod.Dequeue();
        DisclosureNumber(firstPeriodNumber, secondPeriodNumber, RememberFirstNumber[firstPeriodNumber], RememberSecondNumber[secondPeriodNumber]);
    }
    private (int GuessDigit, int[] GuessNumber) IninizializationNumber(int[] GuessNumber, int GuessDigit, Queue<int> RememberOfPeriod, Dictionary<int, int> RememberNumber)
    {
        int _copyhidNumber = _hiddenNumber;
        for (int i = 0; i < GuessNumber.Length; i++)
        {
            int _hidRandomDigit = Random.Range(0, 2);
            GuessNumber[i] = Random.Range(0, 9);
            if (GuessDigit == 0)
                GuessDigit = GuessNumber[i];
            else
                GuessDigit = GuessDigit * 10 + GuessNumber[i];
            if (_hidRandomDigit == 0 && _copyhidNumber != 0)
            {
                RememberOfPeriod.Enqueue(i);
                RememberNumber.Add(i, GuessNumber[i]);
                if (GuessNumber[i] != 5)
                    GuessNumber[i] = 5;
                else
                    GuessNumber[i] = 4;
                _copyhidNumber--;
            }
        }
        return (GuessDigit, GuessNumber);
    }
    private void OnTime()
    {
        int firstPeriodNumber = RememberOfFirstPeriod.Dequeue();
        int secondPeriodNumber = RememberOfFirstPeriod.Dequeue();
        DisclosureNumber(firstPeriodNumber, secondPeriodNumber, RememberFirstNumber[firstPeriodNumber], RememberSecondNumber[secondPeriodNumber]);
    }

    private Transform GetPositionForButton(int index)
    {
        return _arrayTextNumbers[index].transform.parent.transform;
    }

    private (bool,int) CheckWining()
    {
        
        if (_choiseNumber == _FirstGuessDigit)
            return true;
        else
            return false;
    }

    private void PlayersChouse(int hisNumber)
    {
        _choiseNumber[_countOfChouising] = RandomNumbers[hisNumber];
        _countOfChouising++;
    }

    private void DisclosureNumber(int firstKeyNumber, int secondKeyNumber, int firstValeyOfKey, int secondValeyOfKey)
    {
        string FirstNameOfNumber = "";
        string SecondNameOfNumber = "";
        for (int i = 0; i < _FirstGuessNumber.Length; i++)
        {
            int FirstSomeNumber = _FirstGuessNumber[i];
            int SecondSomeNumber = _SecondGuessNumber[i];
            if (firstKeyNumber == i)
            {
                FirstNameOfNumber = FirstNameOfNumber + firstValeyOfKey;
                _FirstGuessNumber[i] = firstValeyOfKey;
            }
            else
            {
                if (FirstSomeNumber == 5 && RememberFirstNumber.ContainsKey(i) && RememberFirstNumber[i] != FirstSomeNumber)
                    FirstNameOfNumber = FirstNameOfNumber + "*";
                else
                    FirstNameOfNumber = FirstNameOfNumber + FirstSomeNumber;
            }
            if (secondKeyNumber == i)
            {
                SecondNameOfNumber = SecondNameOfNumber + secondValeyOfKey;
                _SecondGuessNumber[i] = secondValeyOfKey;
            }
            else
            {
                if (SecondSomeNumber == 5 && RememberSecondNumber.ContainsKey(i) && RememberSecondNumber[i] != SecondSomeNumber)
                    SecondNameOfNumber = SecondNameOfNumber + "*";
                else
                    SecondNameOfNumber = SecondNameOfNumber + SecondSomeNumber;
            }
        }
        _textOfSecondHidNumber.text = SecondNameOfNumber;
        _textOfFirstHidNumber.text = FirstNameOfNumber;
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