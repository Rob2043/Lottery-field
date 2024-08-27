
using StuffEnums;
using UnityEngine;
namespace CustomInterfase
{
    public interface IBuyObject
    {
        public Enums _typeOfStuff { get; set; }
        public string _nameOfStuff { get; set; }
        public GameObject _buyObject { get; set; }
        public int Price { get; set; }
        public bool IsBuy { get; set; }
        public bool IsSelect { get; set; }
        void SetDataOfStuff();
    }
}
