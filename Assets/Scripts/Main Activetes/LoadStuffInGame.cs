using System.Collections.Generic;
using CustomEventBus;
using CustomInterfase;
using StuffEnums;
using UnityEngine;
using UnityEngine.UI;

public class LoadStuffInGame : MonoBehaviour
{
    [SerializeField] private ObjectForBuy[] _objectForBuys;
    [SerializeField] private Image _mainBackgroundImage;
    [SerializeField] private AudioSource _mainAudio;
    private Dictionary<Enums, List<ObjectForBuy>> dataOfStuff = new Dictionary<Enums, List<ObjectForBuy>>(3);

    private void Awake()
    {
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
    }

    private void BuyStuff(Enums enumOfBuyObject, string NameOfStuff)
    {
        List<ObjectForBuy> data = dataOfStuff[enumOfBuyObject];
        for (int i = 0; i < data.Count; i++)
            if (NameOfStuff == data[i]._nameOfObject)
            {
                data[i].IsBuy = true;
                data[i].IsSelect = true;
                switch (enumOfBuyObject)
                {
                    case Enums.music:
                        _mainAudio.clip = data[0].Music;
                        break;
                    case Enums.background:
                        _mainBackgroundImage.sprite = data[i].Background;
                        break;
                }
            }
            else
                data[i].IsSelect = false;
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
