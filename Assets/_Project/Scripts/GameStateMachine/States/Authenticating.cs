using System;
using Pixelplacement;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;

namespace Web3_Skyrim
{
    public class Authenticating : State
    {
        public static event Action<LoginResult> OnSuccess;

        [SerializeField] private GameObject playFabLayout;

        private void OnEnable()
        {
            playFabLayout.SetActive(false);
        }

        public void LoginToPlayFab()
        {
            playFabLayout.SetActive(true);
            
            var request = new LoginWithCustomIDRequest { CustomId = "UnityWeb3Skyrim", CreateAccount = true};
            PlayFabClientAPI.LoginWithCustomID(request, OnLoginSuccess, OnLoginFailure);
        }

        private void OnLoginSuccess(LoginResult result)
        {
            Debug.Log("PlayFab login successful!");
            
            OnSuccess?.Invoke(result); // PlayFab Server will listen to this event
            
            playFabLayout.SetActive(false);
            Next(); // Go to Exploring state
        }

        private void OnLoginFailure(PlayFabError error)
        {
            Debug.LogWarning("Something went wrong with PlayFab Login");
            Debug.LogError("Here's some debug information:");
            Debug.LogError(error.GenerateErrorReport());
        }
    }   
}
