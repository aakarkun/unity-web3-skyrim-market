using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Web3_Skyrim
{
    public class Exchange : ShopBase
    {
        public static Action<int, int> OnTradeExecuted;

        private PlayFabVirtualCurrency _playFabVirtualCurrency;
        
        [Header("Header UI")]
        [SerializeField] private TextMeshProUGUI crystalBalanceLabel;
        
        [Header("Body UI")]
        [SerializeField] private TMP_InputField crystalInput;
        [SerializeField] private TextMeshProUGUI septimLabel;
        [SerializeField] private Button tradeButton;

        private const int MinimumInput = 50;
        private const float ExchangeRate = 1 / (float)MinimumInput; // 1 Septim = 50 Crystals

        private int _currentCrystalBalance;
        private int _crystalInputAmount;
        private int _roundedSeptimAmount;
        

        #region UNITY_LIFECYCLE

        private void Awake()
        {
            title.text = "Exchange";   
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            
            crystalInput.interactable = false;
            ResetData();
            
            // First we create a new instance and we subscribe to the needed event
            _playFabVirtualCurrency = new PlayFabVirtualCurrency();
            _playFabVirtualCurrency.OnGetBalanceSuccess += OnGetBalanceSuccessHandler;
            
            // Now we call the GetBalance function. We will get the balance on the event that we just subscribed
            _playFabVirtualCurrency.GetBalance();
        }

        private void OnDisable()
        {
            ResetData();
            
            _playFabVirtualCurrency.OnGetBalanceSuccess -= OnGetBalanceSuccessHandler;
        }

        #endregion
        

        public void OnTradeButtonPressed()
        {
            // This will be listened by GameManager and we will save the amount of Septim there
            OnTradeExecuted?.Invoke(_crystalInputAmount, _roundedSeptimAmount);
        }


        #region EVENT_HANDLERS

        private void OnGetBalanceSuccessHandler(int currencyBalance)
        {
            _currentCrystalBalance = currencyBalance;
            crystalBalanceLabel.text = _currentCrystalBalance.ToString();

            if (currencyBalance < MinimumInput)
            {
                crystalInput.interactable = false;
                tradeButton.interactable = false;

                statusLabel.text = $"Minimum balance is {MinimumInput} CR";
            }
            else
            {
                // If we have enough balance, we can exchange
                crystalInput.interactable = true;
            }
        }

        #endregion

        public void OnCrystalInputValueChanged()
        {
            // First we try to parse the crystal input field value to an integer
            bool success = int.TryParse(crystalInput.text, out _crystalInputAmount);
            
            if (success)
            {
                // TODO Check with balance //TODOOOO
                // If we are inputting more Crystals than the minimum
                if (_crystalInputAmount >= MinimumInput)
                {
                    if (_crystalInputAmount > _currentCrystalBalance)
                    {
                        statusLabel.text = $"You don't have enough CR";
                        tradeButton.interactable = false;                        
                        return;
                    }
                    
                    // We convert Crystals to Septim using our exchange Rate
                    var septimAmount = _crystalInputAmount * ExchangeRate;
                    
                    //We do a RoundToInt to convert the float septimAmount to int
                    _roundedSeptimAmount = Mathf.RoundToInt(septimAmount);
                    septimLabel.text = _roundedSeptimAmount.ToString();
                    
                    tradeButton.interactable = true;
                    statusLabel.text = string.Empty; // All good
                }
                else
                {
                    statusLabel.text = $"Minimum amount is {MinimumInput} CR";
                    septimLabel.text = 0.ToString();
                    tradeButton.interactable = false;
                }

                Console.WriteLine($"Converted '{crystalInput.text}' to {_crystalInputAmount}.");
            }
            else
            {
                ResetData();
                Console.WriteLine($"Attempted conversion of '{crystalInput.text ?? "<null>"}' failed.");
            }
        }

        private void ResetData()
        {
            crystalInput.text = string.Empty;
            septimLabel.text = 0.ToString();
            _roundedSeptimAmount = 0;

            tradeButton.interactable = false;
        }
    }
}
