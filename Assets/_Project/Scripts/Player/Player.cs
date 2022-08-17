using System;
using UnityEngine;

namespace Web3_Skyrim
{
    public class Player : MonoBehaviour
    {
        [Header("Main Custom Components")]
        public PlayerInputController input;
        public PlayerMovement movement;
        public PlayerOutfitController outfit;
        public PlayerWalletAddress walletAddress;
        
        //Control vars
        [HideInInspector] public Vector3 initPos;


        #region UNITY_LIFECYCLE

        private void Awake()
        {
            initPos = transform.position;
        }

        private void OnEnable()
        {
            PlayerItem.OnSelected += OnPlayerItemSelected;
            DefaultItem.OnSelected += OnDefaultItemSelected;
        }

        private void OnDisable()
        {
            PlayerItem.OnSelected -= OnPlayerItemSelected;
            DefaultItem.OnSelected -= OnDefaultItemSelected;
        }

        #endregion


        #region PUBLIC_METHODS

        public void ShowPlayerWalletAddress()
        {
            walletAddress.Activate();
        }

        #endregion
        
        
        #region EVENT_HANDLERS
        
        private void OnPlayerItemSelected(PlayerItem playerItem)
        {
            outfit.ChangeOutfit(playerItem.GetTexture());
        }
        
        private void OnDefaultItemSelected()
        {
            outfit.DefaultOutfit();
        }
        
        #endregion
    }
}

