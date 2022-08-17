using System;
using PlayFab;
using PlayFab.ServerModels;
using UnityEngine;

namespace Web3_Skyrim
{
    public class PlayFabRevokeItem
    {
        public event Action OnSuccess;

        public void Revoke(string inventoryItemId)
        {
            var request = new RevokeInventoryItemRequest
            {
                ItemInstanceId = inventoryItemId,
                PlayFabId = PlayFabServerSimulator.Instance.playerId
            };
            
            PlayFabServerAPI.RevokeInventoryItem(request, OnRevokeItemSuccess, OnError);
        }

        private void OnRevokeItemSuccess(RevokeInventoryResult result)
        {
            Debug.Log("Revoked ShopInventory item");
        }
        
        private void OnError(PlayFabError error)
        {
            Debug.Log(error);
        }
    }   
}
