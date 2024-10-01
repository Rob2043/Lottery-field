using System.Collections;
using System.Collections.Generic;
using CustomEventBus;
using UnityEngine;

public class CompletingTask : MonoBehaviour
{
    private const int AmountOfTask = 3;
    [SerializeField] private int[] _winigPositions = new int[4];
    private string[] _arrayOfNameTask = new string[AmountOfTask];
    private Dictionary<string, float> _typesOfTask = new(AmountOfTask);
    private void Start()
    {
        _arrayOfNameTask = Iinstance.instance.ArrayOfNameTask;
        _typesOfTask = Iinstance.instance.TypesOfTask;
        EventBus.CheckTask = CheckComptingOfTask;
    }

    private void CheckComptingOfTask(int GuestNumber, int PeriodOfNumber)
    {
        for (int i = 0; i < _typesOfTask.Count; i++)
        {
            switch (i)
            {
                case 1:
                    for (int j = 0; j < _winigPositions.Length; j++)
                        if (_winigPositions[j] == PeriodOfNumber)
                        {
                            PlayerPrefs.SetInt($"{_arrayOfNameTask[i]}ResultOfTask", PlayerPrefs.GetInt($"{_arrayOfNameTask[i]}ResultOfTask", 0) + 1);
                            if( PlayerPrefs.GetInt($"{_arrayOfNameTask[i]}ResultOfTask", 0) == _typesOfTask[_arrayOfNameTask[i]])
                                PlayerPrefs.SetInt($"{_arrayOfNameTask[i]}TaskWasCollecting{i}", 2);
                        }

                    break;
                case 2:
                    int mainNumber = GuestNumber;
                    int countOfNumber = 0;
                    float number = mainNumber;
                    float secondNumber;
                    while (number > 0)
                    {
                        number /= 10;
                        secondNumber = number - (int)number;
                        countOfNumber += (int)(secondNumber * 10);
                    }
                    if (countOfNumber > _typesOfTask[_arrayOfNameTask[i]])
                        PlayerPrefs.SetInt($"{_arrayOfNameTask[i]}TaskWasCollecting{i}", 2);
                    break;
                case 3:
                    int Number = GuestNumber;
                    int countOf = 0;
                    float _number = Number;
                    float secondnumber;
                    while (_number > 0)
                    {
                        _number /= 10;
                        secondnumber = _number - (int)_number;
                        if (secondnumber == 4)
                            countOf++;
                    }
                    if (countOf == _typesOfTask[_arrayOfNameTask[i]])
                        PlayerPrefs.SetInt($"{_arrayOfNameTask[i]}TaskWasCollecting{i}", 2);
                    break;
            }
            PlayerPrefs.Save();
        }
    }
}
