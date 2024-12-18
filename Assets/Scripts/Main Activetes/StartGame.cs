using System.Collections.Generic;
using TMPro;
using UnityEngine;
using CustomEventBus;
using UnityEngine.UIElements;
using UnityEngine.Rendering;

public class StartGame : MonoBehaviour
{
    const int MinCountOfLevel = 10;
    [Header("Texts")]
    [SerializeField] private GameObject[] _ButtonsNumbers = new GameObject[16];
    [SerializeField] private TMP_Text _textOfFirstHidNumber;
    [SerializeField] private RectTransform _moneyPosition;
    [SerializeField] private RectTransform _gamePanelPosition;
    [SerializeField] private RectTransform _timerPosition;
    [SerializeField] private RectTransform _scorePosition;
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
    private int falseNumber = 0;
    private int secondfalseNumber = 0;
    private int _countOfSecretNumbers = 0;
    private int[] _arrayOfSecretsNumbers;
    private int _distanceForSeparation = 10;
    #endregion RandomsNumbers

    private void Awake()
    {
        _textOfSecondHidNumber = _ObjectOfSecondHidNumber.GetComponent<TMP_Text>();
        int MyLevel = Iinstance.instance.MyLevel;
        if (MyLevel <= 2)
        {
            if (EventBus.ChangeUI.Invoke() == true)
            {
                Vector2 textTransform = _textOfFirstHidNumber.GetComponent<RectTransform>().anchoredPosition;
                _textOfFirstHidNumber.GetComponent<RectTransform>().anchoredPosition = new Vector2(textTransform.x + 200f, textTransform.y - 195f);
                _timerPosition.anchoredPosition = new Vector2(_timerPosition.anchoredPosition.x, _timerPosition.anchoredPosition.y - 125f);
                _scorePosition.anchoredPosition = new Vector2(_scorePosition.anchoredPosition.x, _scorePosition.anchoredPosition.y - 75f);
                _moneyPosition.anchoredPosition = new Vector2(_moneyPosition.anchoredPosition.x, _moneyPosition.anchoredPosition.y - 75f);
            }
            else
            {
                Vector2 textTransform = _textOfFirstHidNumber.GetComponent<RectTransform>().anchoredPosition;
                _textOfFirstHidNumber.GetComponent<RectTransform>().anchoredPosition = new Vector2(textTransform.x + 200f, textTransform.y - 50f);
                _gamePanelPosition.anchoredPosition = new Vector2(_gamePanelPosition.anchoredPosition.x, _gamePanelPosition.anchoredPosition.y - 150f);
            }
            switch (MyLevel)
            {
                case 1:
                    _countOfSecretNumbers = 1;
                    break;
                case 2:
                    _countOfSecretNumbers = 2;
                    break;
            }
            _countOfTickets = MinCountOfLevel;
            checklevel = false;
            _choiceNumbers = new int[1];
            _choiceNumbers[0] = _FirstGuessDigit;
            _ObjectOfSecondHidNumber.SetActive(false);
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
            _countOfTickets = MyLevel * 2 + 6;
            _countOfSecretNumbers = MyLevel * 2 - 2;
            Debug.Log(_countOfTickets);
        }
        _arrayOfSecretsNumbers = new int[_countOfSecretNumbers];
        for (int i = 0; i < _countOfSecretNumbers; i++)
        {
            falseNumber = Random.Range(1, _countOfTickets);
            _arrayOfSecretsNumbers[i] = falseNumber;
        }
        _countOfNumbers = Random.Range(7, 9);
        _FirstGuessNumber = new int[_countOfNumbers];
        _SecondGuessNumber = new int[_countOfNumbers];
        RandomNumbers = new int[_countOfTickets];
        (_FirstGuessDigit, _FirstGuessNumber) = IninizializationNumber(_FirstGuessNumber, _FirstGuessDigit, RememberOfFirstPeriod, RememberFirstNumber);
        (_SecondGuessDigit, _SecondGuessNumber) = IninizializationNumber(_SecondGuessNumber, _SecondGuessDigit, RememberOfSecondPeriod, RememberSecondNumber);
        int target = _countOfTickets - 1;
        for (int i = 0; i < _ButtonsNumbers.Length; i++)
        {
            if (i > target)
            {
                _ButtonsNumbers[i].SetActive(false);
            }
            else if (MyLevel < 5)
            {
                if (i % 2 == 0)
                {
                    _distanceForSeparation += 10;
                    Vector2 position = _ButtonsNumbers[i].GetComponent<RectTransform>().anchoredPosition;
                    _ButtonsNumbers[i].GetComponent<RectTransform>().anchoredPosition = new Vector2(position.x, position.y - 240f - _distanceForSeparation);
                }
                else
                {
                    Vector2 position = _ButtonsNumbers[i].GetComponent<RectTransform>().anchoredPosition;
                    Vector2 previousPosition = _ButtonsNumbers[i - 1].GetComponent<RectTransform>().anchoredPosition;
                    _ButtonsNumbers[i].GetComponent<RectTransform>().anchoredPosition = new Vector2(position.x, previousPosition.y);
                }
            }
        }
        bool diferentRandom = false;
        do
        {
            RandomPeriodFirstNumber = Random.Range(0, _countOfTickets);
            RandomPeriodSecondNumber = Random.Range(0, _countOfTickets);
            if (RandomPeriodFirstNumber != RandomPeriodSecondNumber)
                diferentRandom = true;
        } while (diferentRandom == false);
        for (int i = 0; i < RandomNumbers.Length; i++)
        {
            _arrayTextNumbers[i] = _ButtonsNumbers[i].GetComponentInChildren<TMP_Text>();
            int _randomNumber = RandomNumbers[i];
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
            }
            _arrayTextNumbers[i].text = $"{_randomNumber}";
            RandomNumbers[i] = _randomNumber;
        }
        for (int i = 0; i < _arrayOfSecretsNumbers.Length; i++)
        {
            RandomNumbers[i] = ChangeGuestNumber();
            _arrayTextNumbers[i].text = $"{RandomNumbers[i]}";
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
    private int ChangeGuestNumber()
    {
        int _guessNumber = 0;
        switch (Random.Range(1, 3))
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