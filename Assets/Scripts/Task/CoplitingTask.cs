using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoplitingTask : MonoBehaviour
{
    private const int AmountOfTask = 3;
    [SerializeField] private int[] _winigPositions = new int[4];
    private string[] _arrayOfNameTask = new string[AmountOfTask];
    private Dictionary<string, int> _typesOfTask = new(AmountOfTask);
    private void Start()
    {
        _arrayOfNameTask = Iinstance.instance.ArrayOfNameTask;
        _typesOfTask = Iinstance.instance.TypesOfTask;
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
                            PlayerPrefs.SetInt($"{_arrayOfNameTask[i]}ResultOfTask", PlayerPrefs.GetInt($"{_arrayOfNameTask[i]}ResultOfTask", 0) + 1);
                    break;
                case 2:
                    int mainNumber = GuestNumber;
                    int countOfNumber = 0;
                    float number;
                    float secondNumber;
                    while (mainNumber > 0)
                    {
                        number = mainNumber / 10;
                        secondNumber = number - mainNumber;
                        countOfNumber += (int)(secondNumber * 10);
                    }
                    if (countOfNumber > _typesOfTask[_arrayOfNameTask[i]])
                        PlayerPrefs.SetInt($"{_arrayOfNameTask[i]}TaskWasCollecting{i}", 1);
                    break;
                case 3:
                    int Number = GuestNumber;
                    int countOf = 0;
                    float _number;
                    float secondnumber;
                    while (Number > 0)
                    {
                        _number = Number / 10;
                        secondnumber = _number - Number;
                        if(secondnumber == 4)
                            countOf ++;
                    }
                    if(countOf == _typesOfTask[_arrayOfNameTask[i]])
                        PlayerPrefs.SetInt($"{_arrayOfNameTask[i]}TaskWasCollecting{i}", 1);
                    break;
            }
        }
    }
}
