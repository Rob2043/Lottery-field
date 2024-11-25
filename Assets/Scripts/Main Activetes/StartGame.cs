using System.Collections.Generic;
using TMPro;
using UnityEngine;
using CustomEventBus;
using UnityEngine.UIElements;
using UnityEngine.Rendering;

public class StartGame : MonoBehaviour
{
    [Header("Texts")]
    [SerializeField] private GameObject[] _ButtonsNumbers = new GameObject[16];
    [SerializeField] private TMP_Text _textOfFirstHidNumber;
    [SerializeField] private TMP_Text _textCountOfChance;
    [SerializeField] private GameObject _ObjectOfSecondHidNumber;
    private TMP_Text _textOfSecondHidNumber;
    #region Main
    private TMP_Text[] _arrayTextNumbers = new TMP_Text[16];
    private int[] winTikets = new int[4];
    private bool soManyTickets = false;
    private bool checklevel;
    private int _countOfTickets;
    #endregion Main
    #region FirstNumber
    private Dictionary<int, int> RememberFirstNumber = new();
    private Queue<int> RememberOfFirstPeriod = new();
    private int[] _FirstGuessNumber;
    private int _FirstGuessDigit = 0;
    #endregion FirstNumber
    #region  SecondNumber
    private int _SecondGuessDigit = 0;
    private Dictionary<int, int> RememberSecondNumber = new();
    private Queue<int> RememberOfSecondPeriod = new();
    private int[] _SecondGuessNumber;
    #endregion SecondNumber
    #region RandomsNumbers
    private int[] RandomNumbers;
    private int[] _choiceNumbers;
    private int _countOfChoosing = 0;
    private int _countOfNumbers;
    private int _hiddenNumber = 5;
    private int RandomPeriodFirstNumber = 0;
    private int RandomPeriodSecondNumber = 0;
    private int firstfalseNumber = 0;
    private int secondfalseNumber = 0;
    #endregion RandomsNumbers

    private void Awake()
    {
        _textOfSecondHidNumber = _ObjectOfSecondHidNumber.GetComponent<TMP_Text>();
        if (Iinstance.instance.MyLevel <= 5)
        {
            Vector2 textTransform = _textOfFirstHidNumber.GetComponent<RectTransform>().anchoredPosition;
            _textOfFirstHidNumber.GetComponent<RectTransform>().anchoredPosition = new Vector2(textTransform.x + 180f, textTransform.y);
            _choiceNumbers = new int[1];
            _choiceNumbers[0] = _FirstGuessDigit;
            _countOfTickets = Iinstance.instance.MyLevel * 2 + 2;
            _ObjectOfSecondHidNumber.SetActive(false);
            checklevel = false;
            _textCountOfChance.text = "/1";
        }
        else
        {
            _textCountOfChance.text = "/2";
            checklevel = true;
            _choiceNumbers = new int[2];
            _choiceNumbers[0] = _FirstGuessDigit;
            _choiceNumbers[1] = _SecondGuessDigit;
            soManyTickets = true;
            _countOfTickets = Iinstance.instance.MyLevel * 2 + 6;
        }
        _countOfNumbers = Random.Range(7, 9);
        _FirstGuessNumber = new int[_countOfNumbers];
        _SecondGuessNumber = new int[_countOfNumbers];
        RandomNumbers = new int[_countOfTickets];
        (_FirstGuessDigit, _FirstGuessNumber) = IninizializationNumber(_FirstGuessNumber, _FirstGuessDigit, RememberOfFirstPeriod, RememberFirstNumber);
        (_SecondGuessDigit, _SecondGuessNumber) = IninizializationNumber(_SecondGuessNumber, _SecondGuessDigit, RememberOfSecondPeriod, RememberSecondNumber);
        for (int i = 0; i < _ButtonsNumbers.Length; i++)
        {
            _arrayTextNumbers[i] = _ButtonsNumbers[i].GetComponentInChildren<TMP_Text>();
            Vector3 position = _ButtonsNumbers[i].GetComponent<RectTransform>().anchoredPosition;
            if (i > _countOfTickets - 1)
            {
                Debug.Log(i);
                _ButtonsNumbers[i].SetActive(false);
            }    
            else if (Iinstance.instance.MyLevel <= 3)
                _ButtonsNumbers[i].GetComponent<RectTransform>().anchoredPosition = new Vector3(position.x, position.y - 440f, position.z);
        }
        bool diferentRandom = false;
        do
        {
            RandomPeriodFirstNumber = Random.Range(0, _countOfTickets);
            RandomPeriodSecondNumber = Random.Range(0, _countOfTickets);
            if (RandomPeriodFirstNumber != RandomPeriodSecondNumber)
                diferentRandom = true;
        } while (diferentRandom == false);
        bool firstCheck = false;
        bool secondCheck = false;
        do
        {
            firstfalseNumber = Random.Range(1, _countOfTickets);
            if (RandomPeriodFirstNumber != firstfalseNumber)
                firstCheck = true;
            secondfalseNumber = Random.Range(1, _countOfTickets);
            if (RandomPeriodSecondNumber != secondfalseNumber)
                secondCheck = true;
        } while (firstCheck == false & secondCheck == false);
        for (int i = 0; i < RandomNumbers.Length; i++)
        {
            int _randomNumber = RandomNumbers[i];
            if (firstfalseNumber == i)
                _randomNumber = ChangeGuestNumber(1);
            else if (secondfalseNumber == i && soManyTickets!)
                _randomNumber = ChangeGuestNumber(2);
            else
            {
                if (i == RandomPeriodFirstNumber)
                    _randomNumber = _FirstGuessDigit;
                else if (i == RandomPeriodSecondNumber && soManyTickets!)
                    _randomNumber = _SecondGuessDigit;
                else
                {
                    int _guessRandomDigit = 0;
                    for (int j = 0; j < _countOfNumbers; j++)
                    {
                        _guessRandomDigit = Random.Range(1, 10);
                        if (_randomNumber == 0)
                            _randomNumber = _guessRandomDigit;
                        else
                            _randomNumber = (_randomNumber * 10) + _guessRandomDigit;
                    }
                    Debug.Log(_randomNumber);
                }
            }
            _arrayTextNumbers[i].text = $"{_randomNumber}";
            RandomNumbers[i] = _randomNumber;
        }
        OnTime();
    }
    private void OnEnable()
    {
        EventBus.SituationWithCountOfGuessNumber += ReturnBool;
        EventBus.ReturnWinArray += ReturnWinTickets;
        EventBus.GetTransformForBillet += GetPositionForButton;
        EventBus.TimeToUpdateHidNumber += OnTime;
        EventBus.SetPlayersChouse += PlayersChouse;
        EventBus.ReadyForCheck += CheckWining;
    }
    private void OnDisable()
    {
        EventBus.SituationWithCountOfGuessNumber -= ReturnBool;
        EventBus.ReturnWinArray -= ReturnWinTickets;
        EventBus.GetTransformForBillet -= GetPositionForButton;
        EventBus.TimeToUpdateHidNumber -= OnTime;
        EventBus.SetPlayersChouse -= PlayersChouse;
        EventBus.ReadyForCheck -= CheckWining;
    }
    private int[] ReturnWinTickets()
    {
        int count = _countOfTickets - 2;
        for (int i = 2; i < winTikets.Length; i++)
        {
            winTikets[i] = count;
            count++;
        }
        return winTikets;
    }
    private bool ReturnBool()
    {
        return checklevel;
    }
    private int ChangeGuestNumber(int period)
    {
        int _guessNumber = 0;
        switch (period)
        {
            case 1:
                _guessNumber = _FirstGuessDigit;
                break;
            case 2:
                _guessNumber = _SecondGuessDigit;
                break;
        }
        int originalGuessNumber = _guessNumber;
        int _periodNumberForChange = Random.Range(0, _countOfNumbers);
        int _numberForChange = Random.Range(0, _countOfNumbers);
        int currentPosition = 0;
        int resultNumber = 0;
        int multiplier = 1;
        for (; originalGuessNumber > 0; originalGuessNumber /= 10)
        {
            int currentDigit = originalGuessNumber % 10;

            if (currentPosition == _periodNumberForChange)
                currentDigit = _numberForChange;

            resultNumber += currentDigit * multiplier;
            multiplier *= 10;
            currentPosition++;
        }
        return resultNumber;
    }

    private (int GuessDigit, int[] GuessNumber) IninizializationNumber(int[] GuessNumber, int GuessDigit, Queue<int> RememberOfPeriod, Dictionary<int, int> RememberNumber)
    {
        int _copyhidNumber = _hiddenNumber;
        while (_copyhidNumber > 0)
        {
            RememberNumber.Clear();
            RememberOfPeriod.Clear();
            GuessDigit = 0;
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
                    GuessNumber[i] = 9;
                    _copyhidNumber--;
                }
            }
            if (_copyhidNumber != 0)
                _copyhidNumber = _hiddenNumber;
        }
        return (GuessDigit, GuessNumber);
    }
    private void OnTime()
    {
        int firstPeriodNumber = RememberOfFirstPeriod.Dequeue();
        int secondPeriodNumber = RememberOfSecondPeriod.Dequeue();
        DisclosureNumber(firstPeriodNumber, secondPeriodNumber, RememberFirstNumber[firstPeriodNumber], RememberSecondNumber[secondPeriodNumber]);
    }

    private Transform GetPositionForButton(int index)
    {
        return _arrayTextNumbers[index].transform.parent.transform;
    }

    private (bool, int) CheckWining()
    {

        int amountWining = 0;
        bool wasWin = false;
        for (int i = 0; i < _choiceNumbers.Length; i++)
        {
            if (_choiceNumbers[i] == _FirstGuessDigit)
            {
                amountWining++;
                wasWin = true;
            }
            else if (_choiceNumbers[i] == _SecondGuessDigit)
            {
                amountWining++;
                wasWin = true;
            }
            EventBus.CheckTask(_choiceNumbers[i], i);
        }
        return (wasWin, amountWining);
    }

    private void PlayersChouse(int hisNumber)
    {
        if (checklevel == true)
        {
            _choiceNumbers[_countOfChoosing] = RandomNumbers[hisNumber];
            _countOfChoosing++;
        }
        else
        {
            _choiceNumbers[_countOfChoosing] = RandomNumbers[hisNumber];
        }
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
                FirstNameOfNumber += firstValeyOfKey;
                _FirstGuessNumber[i] = firstValeyOfKey;
            }
            else
            {
                if (FirstSomeNumber == 9)
                    FirstNameOfNumber += "*";
                else
                    FirstNameOfNumber += FirstSomeNumber;
            }
            if (secondKeyNumber == i)
            {
                SecondNameOfNumber += secondValeyOfKey;
                _SecondGuessNumber[i] = secondValeyOfKey;
            }
            else
            {
                if (SecondSomeNumber == 9)
                    SecondNameOfNumber += "*";
                else
                    SecondNameOfNumber += SecondSomeNumber;
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