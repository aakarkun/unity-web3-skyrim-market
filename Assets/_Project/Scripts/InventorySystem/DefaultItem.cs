using System;
using UnityEngine;

namespace Web3_Skyrim
{
    public class DefaultItem : MonoBehaviour
    {
        public static event Action OnSelected;

        public void OnClickHandler()
        {
            OnSelected?.Invoke();
        }
    }   
}
