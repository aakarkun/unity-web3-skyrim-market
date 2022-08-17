using System;
using Pixelplacement;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Web3_Skyrim
{
    public class InShopArea : State
    {
        [SerializeField] private TextMeshProUGUI title;
        [SerializeField] private TextMeshProUGUI description;

        private GameStateMachine _gameStateMachine;
        private GameInput _gameInput;

        private void Awake()
        {
            _gameStateMachine = GetComponentInParent<GameStateMachine>();
        }

        private void OnEnable()
        {
            _gameInput = new GameInput();
            
            _gameInput.InShopArea.Enable();
            _gameInput.InShopArea.EnterShop.performed += OnEnterShop;
            
            switch (_gameStateMachine.currentShop)
            {
                case ShopType.Exchange:
                    title.text = "Exchange";
                    description.text = "Here you can trade Crystals (in-game currency) for Septim (ERC-20)";
                    break;
            
                case ShopType.Outfit:
                    title.text = "Outfit Shop";
                    description.text = "Here you can buy NFT Outfits with Septim (ERC-20)";
                    break;
            
                case ShopType.Item:
                    title.text = "Item Shop";
                    description.text = "Here you can buy basic items with Crystals (in-game currency)";
                    break;
            
                case ShopType.None:
                    title.text = "NONE";
                    Debug.Log("Shop type is None");
                    break;
            
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void OnDisable()
        {
            _gameInput.InShopArea.Disable();
            _gameInput.InShopArea.EnterShop.performed -= OnEnterShop;
        }
        
        private void OnEnterShop(InputAction.CallbackContext obj)
        {
            ChangeState("Shopping");
        }
    }   
}
