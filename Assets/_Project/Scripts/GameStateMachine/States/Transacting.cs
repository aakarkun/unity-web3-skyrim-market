using System.Collections;
using Pixelplacement;
using TMPro;
using UnityEngine;

namespace Web3_Skyrim
{
    public class Transacting : State
    {
        [Header("Transactions")] 
        [SerializeField] private ExchangeTransaction exchangeTx;
        [SerializeField] private OutfitTransaction outfitTx;
        
        [Header("UI")]
        [SerializeField] protected TextMeshProUGUI statusLabel;

        // GameStateMachine
        private GameStateMachine _gameStateMachine;
        
        // PlayFab Helper Classes
        private PlayFabVirtualCurrency _playFabVirtualCurrency;
        private PlayFabRevokeItem _playFabRevokeItem;


        #region UNITY_LIFECYCLE

        private void Awake()
        {
            _gameStateMachine = GetComponentInParent<GameStateMachine>();
        }

        private void OnEnable()
        {
            _playFabVirtualCurrency = new PlayFabVirtualCurrency();
            _playFabRevokeItem = new PlayFabRevokeItem();
            
            TransactionBase.OnSuccess += OnTxSuccessHandler;
            TransactionBase.OnFailure += OnTxFailureHandler;
            
            DeactivateAllTransactions();
            
            statusLabel.text = "Please confirm transaction in your wallet"; // We start transacting :)

            switch (_gameStateMachine.currentShop)
            {
                case ShopType.Exchange:
                    exchangeTx.gameObject.SetActive(true);
                    
                    // We get the amount of Septim that we want to exchange/get/mint from the GameManager
                    if (_gameStateMachine.currentSeptimTxAmount <= 0)
                    {
                        Debug.Log("Not enough Septim to mint");
                        return;
                    }
                    exchangeTx.ExchangeCrystalForSeptim(_gameStateMachine.currentSeptimTxAmount);
                    break;
                
                case ShopType.Outfit:

                    var outfitItem = _gameStateMachine.currentOutfitShopItem;
                    
                    outfitTx.gameObject.SetActive(true);
                    outfitTx.BuyOutfit(outfitItem.GetId(), outfitItem.GetPrice(), outfitItem.GetMetadataUrl());
                    break;
                
                case ShopType.Item:
                    // TODO
                    break;
            }
        }
        
        private void OnDisable()
        {
            TransactionBase.OnSuccess -= OnTxSuccessHandler;
            TransactionBase.OnFailure -= OnTxFailureHandler;

            statusLabel.text = string.Empty;
        }

        #endregion


        #region EVENT_HANDLERS

        private void OnTxSuccessHandler(ShopType shopType, string response)
        {
            switch (shopType)
            {
                case ShopType.Exchange:
                    
                    _playFabVirtualCurrency.SubstractAmount(_gameStateMachine.currentCrystalTxAmount);
                    break;
                case ShopType.Outfit:
                    
                    Debug.Log($"{_gameStateMachine.currentOutfitShopItem.GetId()} Outfit bought!");
                    // Now we want to revoke the item from the player's playfab inventory so it doesn't have access to it anymore :)
                    _playFabRevokeItem.Revoke(_gameStateMachine.currentOutfitShopItem.GetId());
                    break;
            }
            
            // TODO Let's wait a bit and show that we succeed
            Debug.Log("Transaction successful. Response :" + response);
            
            statusLabel.text = "Transaction successful!";
            StartCoroutine(ReturnToShopArea());
        }
        
        private void OnTxFailureHandler(ShopType shopType, string response)
        {
            // TODO Let's wait a bit and show that we failed
            Debug.Log("Transaction failed. Response :" + response);
            
            statusLabel.text = "Transaction failed";
            StartCoroutine(ReturnToShopArea());
        }

        #endregion


        #region PRIVATE_METHODS

        private void DeactivateAllTransactions()
        {
            exchangeTx.gameObject.SetActive(false);
            outfitTx.gameObject.SetActive(false);
        }

        private IEnumerator ReturnToShopArea()
        {
            yield return new WaitForSeconds(3f);
            
            ChangeState("InShopArea");
        }

        #endregion
    }   
}
