using System;
using Pixelplacement;
using UnityEngine;

namespace Web3_Skyrim
{
    public enum ShopType
    {
        None,
        Exchange,
        Outfit,
        Item
    }
    
    public class GameStateMachine : StateMachine
    {
        [Header("Main Components")]
        public Player player;

        [Header("Audio")]
        public AudioSource audioSource;
        
        // State control vars
        [HideInInspector] public ShopType currentShop;
        
        // Exchange control vars
        [HideInInspector] public int currentCrystalTxAmount;
        [HideInInspector] public int currentSeptimTxAmount;
        
        // OutfitShop control vars
        [HideInInspector] public ShopItem currentOutfitShopItem;


        #region UNITY_LIFCYCLE

        private void OnEnable()
        {
           ShopCollider.OnPlayerEntered += OnPlayerEnteredHandler;
           
           Exchange.OnTradeExecuted += OnTradeExecutedHandler;
           OutfitShop.OnOutfitSelected += OnOutfitSelectedHandler;
        }

        private void OnDisable()
        {
            ShopCollider.OnPlayerEntered -= OnPlayerEnteredHandler;
            Exchange.OnTradeExecuted -= OnTradeExecutedHandler;
            OutfitShop.OnOutfitSelected -= OnOutfitSelectedHandler;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                Application.Quit();
            }
        }

        #endregion


        #region STATE_MACHINE

        public void OnStateEnteredHandler(GameObject stateEntered)
        {
            switch (stateEntered.name)
            {
                case "Authenticating":
                    player.input.EnableInput(false);
                    break;
                
                case "Exploring":
                    player.input.EnableInput(true);
                    break;
                
                case "Menu":
                    player.input.EnableInput(false);
                    break;
                
                case "InShopArea":
                    player.input.EnableInput(true);
                    break;

                case "Shopping":
                    player.input.EnableInput(false);
                    break;
                
                case "Transacting":
                    player.input.EnableInput(false);
                    break;
            }
        }
        
        public void OnStateExitedHandler(GameObject stateExited)
        {
            switch (stateExited.name)
            {
                case "Authenticating":
                    break;
                
                case "Exploring":
                    break;
                
                case "Menu":
                    break;
            }
        }

        #endregion


        #region PUBLIC_METHODS

        public void GoToExploring()
        {
            player.walletAddress.Activate();
            ChangeState("Exploring");
        }
        
        public void GoToAuthenticating()
        {
            player.walletAddress.Deactivate();
            ChangeState("Authenticating");
        }

        #endregion
        
        
        #region EVENT_HANDLERS
        
        private void OnPlayerEnteredHandler(ShopType shopType, bool playerInside)
        {
            // We always know in what shop we have entered :)
            if (playerInside)
            {
                currentShop = shopType;
                ChangeState("InShopArea");
            }
            else
            {
                currentShop = ShopType.None;
                ChangeState("Exploring");
            }
        }
        
        private void OnTradeExecutedHandler(int crystalAmount, int septimAmount)
        {
            // We save exchange data
            currentCrystalTxAmount = crystalAmount;
            currentSeptimTxAmount = septimAmount;

            ChangeState("Transacting");
        }
        
        private void OnOutfitSelectedHandler(ShopItem item)
        {
            currentOutfitShopItem = item;

            ChangeState("Transacting");
        }
        
        #endregion
        
        
        #region PRIVATE_METHODS
        
        #endregion
    }   
}
