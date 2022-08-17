using Pixelplacement;
using UnityEngine;

// TODO This information could be stored in PlayFab
namespace Web3_Skyrim
{
    public class SmartContracts : Singleton<SmartContracts>
    {
        [Header("Septim Contract (ERC-20) Data")]
        public string septimContractAddress;
    
        [Header("Outfit Contract (ERC-721) Data")]
        public string outfitContractAddress;
    
        [Header("Game Contract Data")] 
        public string gameContractAddress;
        public string gameContractAbi;
    }   
}
