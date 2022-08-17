using System;
using UnityEngine;

namespace Web3_Skyrim
{
    [RequireComponent(typeof(Collider))]
    public class ShopCollider : MonoBehaviour
    {
        public ShopType shopType;

        public static Action<ShopType, bool> OnPlayerEntered;
    
        private void Start()
        {
            Physics.IgnoreLayerCollision(3, 6); // Shop and Environment layers
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                OnPlayerEntered?.Invoke(shopType, true);   
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                OnPlayerEntered?.Invoke(shopType, false);   
            }
        }
    }   
}
