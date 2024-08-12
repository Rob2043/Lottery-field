using System;
using UnityEngine;
using UnityEngine.UI;

namespace CustomEventBus
{
    public static class EventBus
    {
        public static Action TimeToUpdateHidNumber; 
        public static Func<int,Transform> GetTransformForBillet;
        public static Action<int> ChouseNumber; 
        public static Func<int> GetCoins;
        public static Action<int> SetCoins;
        public static Action<int> SetPlayersChouse;
        public static Func<(bool,int)> ReadyForCheck;
        public static Action<string> LodingScene;
    }
}
