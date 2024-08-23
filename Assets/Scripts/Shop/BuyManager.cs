using System.Collections.Generic;
using CustomEventBus;
using CustomInterfase;
using StuffEnums;
using UnityEngine;

public class BuyManager : MonoBehaviour
{
    [SerializeField] private GameObject[] _arrayOfStuff;
    [SerializeField] private ObjectForBuy[] _objectForBuys;
    private Dictionary<Enums, List<ObjectForBuy>> dataOfStuff = new Dictionary<Enums, List<ObjectForBuy>>(3);
    IBuyObject buyObject;

    private void Awake()
    {
        EventBus.BuyAction = BuyStuff;
        LoadData();
        UpdateData();
    }

    private void UpdateData()
    {
        for (int i = 0; i < _arrayOfStuff.Length; i++)
        {
            if (_arrayOfStuff[i].TryGetComponent(out buyObject))
            {
                buyObject._typeOfStuff = _objectForBuys[i].enums;
                buyObject.IsBuy = _objectForBuys[i].IsBuy;
                buyObject.IsSelect = _objectForBuys[i].IsSelect;
                buyObject.Price = _objectForBuys[i]._price;
                buyObject._buyObject = _objectForBuys[i]._object;
                buyObject._nameOfStuff = _objectForBuys[i]._object.name;
                buyObject.SetDataOfStuff();
            }
        }
    }
    private void BuyStuff(Enums enumOfBuyObject, string NameOfStuff, bool WasSelect)
    {
        List<ObjectForBuy> data = dataOfStuff[enumOfBuyObject];
        for (int i = 0; i < data.Count; i++)
        {
            if (NameOfStuff == data[i]._nameOfObject)
            {
                data[i].IsBuy = true;
                if (WasSelect == true)
                {
                    data[i].IsSelect = true;
                    GameObject gameObject = Instantiate(data[i]._object);
                    Iinstance.instance.QueueForBuingStuff.Enqueue(gameObject);
                }
                else
                {
                    data[i].IsSelect = false;
                    GameObject gameObject = Iinstance.instance.QueueForBuingStuff.Dequeue();
                    Destroy(gameObject);
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
                    GameObject gameObject = Instantiate(_objectForBuys[i]._object);
                    Iinstance.instance.QueueForBuingStuff.Enqueue(gameObject);
                }
                else
                {
                    _objectForBuys[i].IsSelect = false;
                    GameObject gameObject = Iinstance.instance.QueueForBuingStuff.Dequeue();
                    Destroy(gameObject);
                }
            }
            else
            {
                _objectForBuys[i].IsBuy = true;
            }
        }
    }
}
