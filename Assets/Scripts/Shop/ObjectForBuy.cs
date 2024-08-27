using StuffEnums;
using UnityEngine;

[CreateAssetMenu(fileName = "ObjectForBuy", menuName = "ObjectForBuy", order = 0)]
public class ObjectForBuy : ScriptableObject
{
    [SerializeField] public Enums enums;
    [SerializeField] public AudioClip Music;
    [SerializeField] public Sprite Background;
    [SerializeField] public int AmountOfFreeSpins;
    [SerializeField] public string _nameOfObject;
    [SerializeField] public int _price;
    public bool IsBuy;
    public bool IsSelect;
}