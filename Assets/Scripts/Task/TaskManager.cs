using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskManager : MonoBehaviour
{
    private const int AmountOfTask = 3;
    [Header("Options For Algorithm")]
    [SerializeField] private string[] _arrayOfNameTask = new string[AmountOfTask];
    [SerializeField] private int[] _arrayOfBasicReward = new int[AmountOfTask];
    [SerializeField] private int[] _arrayOfBasicGoat = new int[AmountOfTask];
    private Dictionary<string, int> _typesOfTask = new(AmountOfTask);
    private int[] _arrayOfReward = new int[AmountOfTask];

    private void Start()
    {
        Iinstance.instance.ArrayOfNameTask = _arrayOfNameTask;
        UpdateTask();
        Iinstance.instance.TypesOfTask = _typesOfTask;

    }
    private void UpdateTask()
    {
        for (int i = 0; i < _typesOfTask.Count; i++)
        {
            int randomNumber = 0;
            switch (i)
            {
                case 1:
                    randomNumber = Random.Range(1, 9);
                    break;
                case 2:
                    randomNumber = Random.Range(10, 81);
                    break;
                case 3:
                    randomNumber = Random.Range(1, 6);
                    break;
            }
            if (PlayerPrefs.GetInt($"{_arrayOfNameTask[i]}TaskWasCollecting{i}", 0) == 1)
            {
                _typesOfTask.Add(_arrayOfNameTask[i], randomNumber);
                PlayerPrefs.SetInt($"LastRandomNumber{i}", randomNumber);
            }
            else
            {
                //_ResultOfTask.Add(_arrayOfNameTask[i], PlayerPrefs.GetInt($"{_arrayOfNameTask[i]}ResultOfTask",0));
                randomNumber = PlayerPrefs.GetInt($"LastRandomNumber{i}");
                _typesOfTask.Add(_arrayOfNameTask[i], randomNumber);
            }
            int percent = (randomNumber / _arrayOfBasicGoat[i]);
            _arrayOfReward[i] = _arrayOfBasicReward[i] * percent;
            PlayerPrefs.Save();
        }
    }

    
}
