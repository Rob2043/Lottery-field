using System;
using System.Collections.Generic;
using CustomEventBus;
using CustomInterfase;
using StuffEnums;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BuyManager : MonoBehaviour
{
    [SerializeField] private GameObject[] _arrayOfStuff;
    [SerializeField] private ObjectForBuy[] _objectForBuys;
    [SerializeField] private Image _mainBackgroundImage;
    [SerializeField] private AudioSource _mainAudio;
    [SerializeField] private TMP_Text _textOfMoney;

    private Dictionary<Enums, List<ObjectForBuy>> dataOfStuff = new Dictionary<Enums, List<ObjectForBuy>>(3);
    IBuyObject buyObject;

    private void Awake()
    {
        _textOfMoney.text = $"{EventBus.GetCoins.Invoke()}";
        EventBus.BuyAction = BuyStuff;
        for (int i = 0; i < _objectForBuys.Length; i++)
        {
            List<ObjectForBuy> newList = new();
            newList.Add(_objectForBuys[i]);
            if (dataOfStuff.ContainsKey(_objectForBuys[i].enums))
                dataOfStuff[_objectForBuys[i].enums].Add(_objectForBuys[i]);
            else dataOfStuff.Add(_objectForBuys[i].enums, newList);
        }
        LoadData();
        UpdateData();
    }

    private void UpdateData()
    {
        _textOfMoney.text = $"{EventBus.GetCoins.Invoke()}";
        for (int i = 0; i < _arrayOfStuff.Length; i++)
        {
            if (_arrayOfStuff[i].TryGetComponent(out buyObject))
            {
                buyObject._typeOfStuff = _objectForBuys[i].enums;
                buyObject.IsBuy = _objectForBuys[i].IsBuy;
                buyObject.IsSelect = _objectForBuys[i].IsSelect;
                buyObject.Price = _objectForBuys[i]._price;
                buyObject._nameOfStuff = _objectForBuys[i]._nameOfObject;
                buyObject.SetDataOfStuff();
            }
        }
    }
    private void BuyStuff(Enums enumOfBuyObject, string NameOfStuff)
    {
        List<ObjectForBuy> data = dataOfStuff[enumOfBuyObject];
        if (enumOfBuyObject == Enums.freespin)
        {
            for (int i = 0; i < data.Count; i++)
                if (NameOfStuff == data[i]._nameOfObject)
                    EventBus.AddFreeSpin.Invoke(data[i].AmountOfFreeSpins);
        }
        else
        {
            for (int i = 0; i < data.Count; i++)
                if (NameOfStuff == data[i]._nameOfObject)
                {
                    data[i].IsBuy = true;
                    data[i].IsSelect = true;
                    switch (enumOfBuyObject)
                    {
                        case Enums.music:
                            _mainAudio.clip = data[0].Music;
                            _mainAudio.Play();
                            break;
                        case Enums.background:
                            _mainBackgroundImage.sprite = data[i].Background;
                            break;
                    }
                }
                else
                    data[i].IsSelect = false;
        }
        UpdateData();
        SaveData();
    }
    private void SaveData()
    {
        for (int i = 0; i < _objectForBuys.Length; i++)
        {
            if (_objectForBuys[i].IsBuy == true)
            {
                PlayerPrefs.SetInt($"{_objectForBuys[i].name}WasBuy", 1);
                if (_objectForBuys[i].IsSelect == true)
                    PlayerPrefs.SetInt($"{_objectForBuys[i].name}WasSelect", 1);
                else
                    PlayerPrefs.SetInt($"{_objectForBuys[i].name}WasSelect", 0);
            }
            else
                PlayerPrefs.SetInt($"{_objectForBuys[i].name}WasBuy", 0);
        }
        PlayerPrefs.Save();
    }
    private void LoadData()
    {
        for (int i = 0; i < _objectForBuys.Length; i++)
        {
            if (PlayerPrefs.GetInt($"{_objectForBuys[i].name}WasBuy", 0) == 1)
            {
                _objectForBuys[i].IsBuy = true;
                if (PlayerPrefs.GetInt($"{_objectForBuys[i].name}WasSelect", 0) == 1)
                {
                    _objectForBuys[i].IsSelect = true;
                    BuyStuff(_objectForBuys[i].enums, _objectForBuys[i]._nameOfObject);
                }
                else
                    _objectForBuys[i].IsSelect = false;
            }
            else _objectForBuys[i].IsBuy = false;
        }
    }
}
