using System.Collections;
using System.Collections.Generic;
using System.Data;
using CustomEventBus;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TaskManager : MonoBehaviour
{
    private const int AmountOfTask = 3;
    [Header("UI")]
    [SerializeField] private Button[] _buttonsForGetMoney = new Button[AmountOfTask];
    [SerializeField] private TMP_Text[] _rewardsTexts = new TMP_Text[AmountOfTask];
    [SerializeField] private TMP_Text[] _goatTexts = new TMP_Text[AmountOfTask];
    private Image[] _imagesFromButton = new Image[AmountOfTask];
    [Header("Options For Algorithm")]
    [SerializeField] private string[] _arrayOfNameTask = new string[AmountOfTask];
    [SerializeField] private float[] _arrayOfBasicReward = new float[AmountOfTask];
    [SerializeField] private int[] _arrayOfBasicGoat = new int[AmountOfTask];
    private Dictionary<string, float> _typesOfTask = new(AmountOfTask);
    private int[] _arrayOfReward = new int[AmountOfTask];
    int count = 0;
    float chooseNumber = 0f;
    private void Start()
    {
        for (int i = 0; i < _imagesFromButton.Length; i++)
            _imagesFromButton[i] = _buttonsForGetMoney[i].gameObject.GetComponent<Image>();
        //PlayerPrefs.DeleteAll();
        Iinstance.instance.ArrayOfNameTask = _arrayOfNameTask;
        Iinstance.instance.TypesOfTask = _typesOfTask;
        for (int i = 0; i < _buttonsForGetMoney.Length; i++)
        {
            _buttonsForGetMoney[i].interactable = false;
            _imagesFromButton[i].color = Color.gray;
        }
        UpdateTask();
    }
    private void UpdateTask()
    {
        while (count < AmountOfTask)
        {
            float randomNumber = 0;
            switch (count)
            {
                case 0:
                    randomNumber = Random.Range(1, 9);
                    break;
                case 1:
                    randomNumber = Random.Range(10, 81);
                    break;
                case 2:
                    randomNumber = Random.Range(1, 6);
                    break;
            }
            if (PlayerPrefs.GetInt($"{_arrayOfNameTask[count]}TaskWasCollecting{count}", 0) == 0)
            {
                _typesOfTask.Add(_arrayOfNameTask[count], randomNumber);
                PlayerPrefs.SetInt($"{_arrayOfNameTask[count]}TaskWasCollecting{count}", 1);
                PlayerPrefs.SetFloat($"LastRandomNumber{count}", randomNumber);
            }
            else if (PlayerPrefs.GetInt($"{_arrayOfNameTask[count]}TaskWasCollecting{count}", 0) == 1)
            {
                float number = PlayerPrefs.GetFloat($"LastRandomNumber{count}", 0);
                _typesOfTask.Add(_arrayOfNameTask[count], number);
                PlayerPrefs.SetFloat($"LastRandomNumber{count}", number);
                randomNumber = number;
            }
            else
            {
                _buttonsForGetMoney[count].interactable = true;
                _imagesFromButton[count].color = Color.white;
            }
            float percent = randomNumber / _arrayOfBasicGoat[count];
            _arrayOfReward[count] = (int)(_arrayOfBasicReward[count] * percent);
            _rewardsTexts[count].text = $"{_arrayOfReward[count]}";
            _goatTexts[count].text = $"{randomNumber}";
            PlayerPrefs.Save();
            count++;
        }
        count = 0;
    }

    public void GiveMoney(int periodOfTask)
    {
        EventBus.SetCoins.Invoke(_arrayOfReward[periodOfTask]);
        EventBus.UpdateMoney.Invoke();
        switch (periodOfTask)
        {
            case 0:
                chooseNumber = Random.Range(1, 9);
                break;
            case 1:
                chooseNumber = Random.Range(10, 81);
                break;
            case 2:
                chooseNumber = Random.Range(1, 6);
                break;
        }
        PlayerPrefs.SetFloat($"LastRandomNumber{periodOfTask}", chooseNumber);
        _typesOfTask[_arrayOfNameTask[periodOfTask]] = chooseNumber;
        float percent = chooseNumber / _arrayOfBasicGoat[periodOfTask];
        _arrayOfReward[periodOfTask] = (int)(_arrayOfBasicReward[periodOfTask] * percent);
        _rewardsTexts[periodOfTask].text = $"{_arrayOfReward[periodOfTask]}";
        _goatTexts[periodOfTask].text = $"{chooseNumber}";
        chooseNumber = 0;
        _buttonsForGetMoney[periodOfTask].interactable = false;
        _imagesFromButton[periodOfTask].color = Color.gray;
        PlayerPrefs.SetInt($"{_arrayOfNameTask[periodOfTask]}TaskWasCollecting{periodOfTask}", 1);
        PlayerPrefs.Save();
    }
}
