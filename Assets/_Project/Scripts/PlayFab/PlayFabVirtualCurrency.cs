using System;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;

namespace Web3_Skyrim
{
    public class PlayFabVirtualCurrency
    {
        public event Action<int> OnGetBalanceSuccess;
        
        
        public void GetBalance()
        {
            PlayFabClientAPI.GetUserInventory(new GetUserInventoryRequest(), OnSuccess, OnError);
        }

        public void SubstractAmount(int amount)
        {
            var request = new SubtractUserVirtualCurrencyRequest()
            {
                VirtualCurrency = PlayFabServerSimulator.Instance.currencyCode,
                Amount = amount
            };
            
            PlayFabClientAPI.SubtractUserVirtualCurrency(request, 
                result =>
                {
                    // We assume this will work well. You should handle this with some callbacks like we do in GetBalance()
                    Debug.Log(amount + " CR substracted!");
                }, error =>
                {
                    Debug.Log("Crystal substraction failed");
                });
        }   

        private void OnSuccess(GetUserInventoryResult result)
        {
            int coins = result.VirtualCurrency[PlayFabServerSimulator.Instance.currencyCode];
            OnGetBalanceSuccess?.Invoke(coins);
        }
        
        private void OnError(PlayFabError error)
        {
            Debug.Log("We could get the balance, check CurrencyCode in PlayFabServerSimulator");
            Debug.Log(error);
        }
    }   
}
