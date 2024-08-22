using UnityEngine;

[CreateAssetMenu(fileName = "ObjectForBuy", menuName = "ObjectForBuy", order = 0)]
public class ObjectForBuy : ScriptableObject
{
    [SerializeField] private GameObject _object;
    [SerializeField] private int _price;
    public bool IsBuy;
    public bool IsSelect;
}