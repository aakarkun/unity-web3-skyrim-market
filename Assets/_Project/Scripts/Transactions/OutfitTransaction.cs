using Cysharp.Threading.Tasks;
using MoralisUnity;
using Nethereum.Hex.HexTypes;

namespace Web3_Skyrim
{
    public class OutfitTransaction : TransactionBase
    {
        public async void BuyOutfit(string tokenId, int price, string metadataUrl)
        {
            var result = await ExecuteOutfitBuy(tokenId, price, metadataUrl);
    
            if (result is null)
            {
                OnFailure?.Invoke(shopType, null);
                return;
            }
    
            // We tell the GameManager what we minted the item successfully
            OnSuccess?.Invoke(shopType, result);
        }
    
        private async UniTask<string> ExecuteOutfitBuy(string tokenId, int price, string tokenUrl)
        {
            var longTokenId = Web3Tools.ConvertStringToLong(tokenId);
            int intTokenId = (int) longTokenId;
            
            object[] parameters = {
                intTokenId.ToString("x"), // To Hexadecimal. This is what the contract expects
                price,
                tokenUrl
            };
    
            // Set gas estimate
            HexBigInteger value = new HexBigInteger(0);
            HexBigInteger gas = new HexBigInteger(0);
            HexBigInteger gasPrice = new HexBigInteger(0);
    
            string resp = await Moralis.ExecuteContractFunction(
                SmartContracts.Instance.gameContractAddress, SmartContracts.Instance.gameContractAbi, "buyOutfit", parameters, value, gas, gasPrice);
    
            return resp;
        }
    }
}
