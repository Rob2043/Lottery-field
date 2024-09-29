using System;
using System.Collections;
using StuffEnums;
using UnityEngine;
using UnityEngine.UI;

namespace CustomEventBus
{
    public static class EventBus
    {
        public static Action TimeToUpdateHidNumber;
        public static Func<int, Transform> GetTransformForBillet;
        public static Action<int> ChouseNumber;
        public static Func<int> GetCoins;
        public static Action<int> SetCoins;
        public static Action<int> SetPlayersChouse;
        public static Func<(bool, int)> ReadyForCheck;
        public static Func<bool> CanPlay;
        public static Action<string> LodingScene;
        public static Action<Enums, string, bool> BuyAction;
        public static Action<int> AddFreeSpin;
        public static Action<bool> FreeSpin;
        public static Action UpdateMoney;
        public static Action<int, int> CheckTask;
        public static Action NextTextForTutorial;
        public static Action StartGame;
    }
}
