using System.Collections;
using System.Collections.Generic;
using System.Data;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;

public class MainChoise : MonoBehaviour
{
    [SerializeField] private float _time;
    [SerializeField] private TMP_Text[] _arrayTextNumbers = new TMP_Text[8];
    [SerializeField] private TMP_Text _textOfGuessNumber;
    [SerializeField] private TMP_Text _textOfHidNumber;
    private Dictionary<int, int> RemeberNumber = new Dictionary<int, int>();
    private int[] RandomNumbers = new int[8];
    private int _choiseNumber = 0;
    private int _countOfNumbers;
    private int[] _guessNumnber;
    private int _guessDigit = 0;
    private int _hiddenNumber = 3;

    private void Awake()
    {
        _countOfNumbers = Random.Range(6, 8);
        _guessNumnber = new int[_countOfNumbers];
        _hiddenNumber = _countOfNumbers switch
        {
            6 => 3,
            8 => 4,
            _ => _hiddenNumber
        };
        int _copyHiddenNumber = _hiddenNumber;
        for (int i = 0; i < _guessNumnber.Length; i++)
        {
            int _hidRandomDigit = Random.Range(0, 1);
            _guessNumnber[i] = Random.Range(0, 9);
            if (_guessDigit == 0)
                _guessDigit = _guessNumnber[i];
            else
                _guessDigit = _guessDigit * 10 + _guessNumnber[i];
            if (_hidRandomDigit == 0 && _copyHiddenNumber != 0)
            {
                RemeberNumber.Add(i, _guessNumnber[i]);
                _guessNumnber[i] = 5;
                _copyHiddenNumber--;
            }
        }
        _textOfGuessNumber.text = $"{_guessDigit}";
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
    }

    private int ToNummber(int[] EnterArray)
    {
        int _hidNumber = 0;
        for (int i = 0; i < EnterArray.Length; i++)
        {
            if (_hidNumber == 0)
                _hidNumber = EnterArray[i];
            else
                _hidNumber = _hidNumber * 10 + _guessNumnber[i];
        }
        return _hidNumber;
    }
}
