using Pixelplacement;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Web3_Skyrim
{
    [RequireComponent(typeof(AudioSource))]
    public class Shopping : State
    {
        [Header("Shops")] 
        [SerializeField] private ShopBase exchange;
        [SerializeField] private ShopBase outfitShop;
        [SerializeField] private ShopBase itemShop;

        private GameStateMachine _gameStateMachine;
        private GameInput _gameInput;
        private AudioSource _audioSource;
        
        private void Awake()
        {
            _gameStateMachine = GetComponentInParent<GameStateMachine>();
            _audioSource = GetComponent<AudioSource>();
        }

        private void OnEnable()
        {
            _gameInput = new GameInput();
            _gameInput.Shopping.Enable();
            _gameInput.Shopping.ExitShop.performed += OnExitShop;
            
            CloseAllShops();
            
            _audioSource.Play();
            
            switch (_gameStateMachine.currentShop)
            {
                case ShopType.Exchange:
                    exchange.gameObject.SetActive(true);
                    break;
                
                case ShopType.Outfit:
                    outfitShop.gameObject.SetActive(true);
                    break;
                
                case ShopType.Item:
                    itemShop.gameObject.SetActive(true);
                    break;
            }
        }

        private void OnDisable()
        {
            _gameInput.Shopping.Disable();
            _gameInput.Shopping.ExitShop.performed -= OnExitShop;
            
            CloseAllShops();
        }
        
        private void OnExitShop(InputAction.CallbackContext obj)
        {
            Previous(); // Go to InShopArea state
        }

        private void CloseAllShops()
        {
            exchange.gameObject.SetActive(false);
            outfitShop.gameObject.SetActive(false);
            itemShop.gameObject.SetActive(false);
        }
    }   
}
