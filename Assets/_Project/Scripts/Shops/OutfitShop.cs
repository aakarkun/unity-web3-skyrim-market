using System;
using System.Collections.Generic;
using System.Linq;
using MoralisUnity;
using MoralisUnity.Web3Api.Models;
using PlayFab;
using PlayFab.ClientModels;
using TMPro;
using UnityEngine;

namespace Web3_Skyrim
{
    public class OutfitShop : ShopBase
    {
        public static event Action<ShopItem> OnOutfitSelected;
            
        [Header("Inventory")]
        public ShopInventory shopInventory;

        [Header("Septim Balance")] 
        [SerializeField] private TextMeshProUGUI septimBalanceLabel;
        
        private ShopItem _currentSelectedItem;
        private int _currentSeptimBalance;
        private string _walletAddress;
        
        
        #region UNITY_LIFECYLE

        private void Awake()
        {
            title.text = "Outfit Shop";
        }

        protected override void OnEnable()
        { 
            base.OnEnable();
            
            ShopItem.OnSelected += OnShopItemSelectedHandler; // We listen to when any shop item is selected
            
            GetSeptimBalance();
            GetUserInventory();
        }

        private void OnDisable()
        {
            ShopItem.OnSelected -= OnShopItemSelectedHandler;
            
            // We could create a system to contrast PlayFab Inventory Items with local items
            // but since it's a demo we just clear all local items every time
            shopInventory.ClearAllItems();
        }

        #endregion


        #region EVENT_HANDLERS

        private void OnShopItemSelectedHandler(ShopItem item)
        {
            if (_currentSeptimBalance >= item.GetPrice())
            {
                // GameManager will listen to this and it will take care of going to Transacting state
                OnOutfitSelected?.Invoke(item);
            }
            else
            {
                statusLabel.text = "Not enough Septim (ST) balance";
            }
        }

        #endregion
        

        private void GetUserInventory()
        {
            PlayFabClientAPI.GetUserInventory(new GetUserInventoryRequest(),
                OnGetUserInventorySuccess,
                OnGetUserInventoryFailure);
        }

        private void OnGetUserInventorySuccess(GetUserInventoryResult result)
        {
            foreach (var inventoryItem in result.Inventory)
            {
                ContrastWithCatalog(inventoryItem);
            }
        }
        
        private void OnGetUserInventoryFailure(PlayFabError error)
        {
            Debug.LogError(error.GenerateErrorReport());
        }
        
        private void ContrastWithCatalog(ItemInstance itemInstance)
        {
            var request = new GetCatalogItemsRequest()
            {
                CatalogVersion = PlayFabServerSimulator.Instance.catalogName
            };
            
            PlayFabClientAPI.GetCatalogItems(request,
                result =>
                {
                    // Check if the player inventory item exists in the Catalog to get all the information we need
                    // We do that because some information is only saved in the CatalogItem and not in the ItemInstance from the player's inventory :(
                    foreach (var catalogItem in result.Catalog)
                    {
                        if (catalogItem.ItemId == itemInstance.ItemId)
                        {
                            // Deserialize the CustomData in the CatalogItem (containing the price) to MetadataObject
                            var customData = catalogItem.CustomData;
                            MetadataObject metadataObject = Web3Tools.DeserializeUsingNewtonSoftJson(customData);
                    
                            // We ONLY want objects with attributes. If metadataObject is null or metadataObject.attributes is null, we don't continue
                            if (metadataObject?.attributes is null)
                            {
                                Debug.Log("Item has no attributes");
                                return;
                            }
                            
                            // Then we populate it to the shop
                            shopInventory.PopulateShopItem(
                                itemInstance.ItemInstanceId,
                                metadataObject, 
                                catalogItem.Description); // We saved the metadata URL in the description
                        }
                    }
                },
                error =>
                {
                    Debug.LogError(error.GenerateErrorReport());
                });
        }
        
        private async void GetSeptimBalance()
        {
            _walletAddress = await Web3Tools.GetWalletAddress();

            if (_walletAddress is null)
            {
                Debug.Log("We need the wallet address to continue");
                return;
            }
            
            // We get our Septim balance
            List<Erc20TokenBalance> listOfTokens = await Moralis.Web3Api.Account.GetTokenBalances(_walletAddress, Moralis.CurrentChain.EnumValue);
            if (!listOfTokens.Any()) return;
            
            foreach (var token in listOfTokens)
            {
                // We make the sure that is the token that we deployed
                if (token.TokenAddress == SmartContracts.Instance.septimContractAddress.ToLower())
                {
                    septimBalanceLabel.text = token.Balance;
                    _currentSeptimBalance = int.Parse(token.Balance); // We assume token.Balance is a number string :)
                    Debug.Log($"We own {token.Balance} Septim (ST)");
                }
            }
        }
    }   
}
