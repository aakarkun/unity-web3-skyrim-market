using System;
using TMPro;
using UnityEngine;

namespace Web3_Skyrim
{
    public class TransactionBase : MonoBehaviour
    {
        public static Action<ShopType, string> OnSuccess;
        public static Action<ShopType, string> OnFailure;
    
        public ShopType shopType;
    }   
}
