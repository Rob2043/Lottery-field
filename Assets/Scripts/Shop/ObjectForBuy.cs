using StuffEnums;
using UnityEngine;

[CreateAssetMenu(fileName = "ObjectForBuy", menuName = "ObjectForBuy", order = 0)]
public class ObjectForBuy : ScriptableObject
{
    [SerializeField] public Enums enums;
    [SerializeField] public GameObject _object;
    [SerializeField] public string _nameOfObject;
    [SerializeField] public int _price;
    public bool IsBuy;
    public bool IsSelect;
}