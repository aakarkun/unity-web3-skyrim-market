using Cysharp.Threading.Tasks;
using MoralisUnity;
using Nethereum.Hex.HexTypes;

namespace Web3_Skyrim
{
    public class ExchangeTransaction : TransactionBase
    {
        public async void ExchangeCrystalForSeptim(int crystalAmount)
        {
            var result = await ExecuteExchange(crystalAmount);
    
            if (result is null)
            {
                OnFailure?.Invoke(shopType, null);
                return;
            }
    
            // We tell the GameManager what we minted the item successfully
            OnSuccess?.Invoke(shopType, result);
        }
    
        private async UniTask<string> ExecuteExchange(int crystalAmount)
        {
            object[] parameters = {
                crystalAmount // This is what the contract expects
            };
    
            // Set gas estimate
            HexBigInteger value = new HexBigInteger(0);
            HexBigInteger gas = new HexBigInteger(0);
            HexBigInteger gasPrice = new HexBigInteger(0);
    
            string resp = await Moralis.ExecuteContractFunction(
                SmartContracts.Instance.gameContractAddress, SmartContracts.Instance.gameContractAbi, "addSeptim", parameters, value, gas, gasPrice);
    
            return resp;
        }
    }
}
