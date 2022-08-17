using System.Collections.Generic;
using System.Linq;
using MoralisUnity;
using MoralisUnity.Kits.AuthenticationKit;
using Pixelplacement;
using UnityEngine;
using UnityEngine.InputSystem;
using MoralisUnity.Web3Api.Models;
using TMPro;

namespace Web3_Skyrim
{
    public class Menu : State
    {
        [Header("Components")]
        [SerializeField] private AuthenticationKit authenticationKit;
        [SerializeField] private PlayerInventory playerInventory;

        [Header("UI")]
        [SerializeField] private TextMeshProUGUI crystalBalanceLabel;
        [SerializeField] private TextMeshProUGUI septimBalanceLabel;

        private GameInput _gameInput;
        private PlayFabVirtualCurrency _playFabVirtualCurrency;
        
        private string _walletAddress;

        private void Awake()
        {
            _gameInput = new GameInput();
        }

        private void OnEnable()
        {
            _gameInput.Menu.Enable();
            _gameInput.Menu.CloseMenu.performed += OnCloseMenu;

            // We get our Crystal balance
            _playFabVirtualCurrency = new PlayFabVirtualCurrency();
            _playFabVirtualCurrency.OnGetBalanceSuccess += SetCrystalBalanceValue;
            _playFabVirtualCurrency.GetBalance();
            
            GetSeptimBalance();
            
            playerInventory.LoadPurchasedOutfits();
        }

        private void OnDisable()
        {
            _gameInput.Menu.Disable();
            _gameInput.Menu.CloseMenu.performed -= OnCloseMenu;
            
            _playFabVirtualCurrency.OnGetBalanceSuccess -= SetCrystalBalanceValue;
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
                    Debug.Log($"We own {token.Balance} Septim (ST)");
                }
            }
        }

        private void SetCrystalBalanceValue(int crystalBalance)
        {
            crystalBalanceLabel.text = crystalBalance.ToString();
            Debug.Log($"We own {crystalBalance} Crystal (CR)");
        }

        private void OnCloseMenu(InputAction.CallbackContext obj)
        {
            Previous();
        }

        public void OnDisconnectPressed()
        {
            authenticationKit.Disconnect();
        }
    }   
}
