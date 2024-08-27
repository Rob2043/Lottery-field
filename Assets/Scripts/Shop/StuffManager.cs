using System.Collections;
using System.Collections.Generic;
using CustomEventBus;
using CustomInterfase;
using TMPro;
using UnityEngine;
using StuffEnums;

public class StuffManager : MonoBehaviour, IBuyObject
{
    public Enums _typeOfStuff { get; set; }
    public GameObject _buyObject { get; set; }
    public int Price { get; set; }
    public bool IsBuy { get; set; }
    public string _nameOfStuff { get; set; }
    public bool IsSelect { get; set; }
    [SerializeField] private TMP_Text _textFotBuy;

    public void SetDataOfStuff()
    {
        if (IsBuy == false)
            _textFotBuy.text = $"Buy {Price}";
        else if (IsSelect == false)
            _textFotBuy.text = $"Select";
        else
            _textFotBuy.text = $"Selcted";
    }

    public void Buy()
    {
        if (IsBuy == false)
        {
            if (Price <= EventBus.GetCoins.Invoke())
            {
                _textFotBuy.text = $"Selct";
                EventBus.BuyAction.Invoke(_typeOfStuff, _nameOfStuff, IsBuy);
                EventBus.SetCoins.Invoke(-Price);
            }
        }
        else if (IsSelect == false)
        {
            EventBus.BuyAction.Invoke(_typeOfStuff, _nameOfStuff, IsBuy);
            _textFotBuy.text = $"Selcted";
        }
    }
}
