using System;
using System.Collections.Generic;
using System.Linq;
using MoralisUnity;
using MoralisUnity.Web3Api.Models;
using UnityEngine;

namespace Web3_Skyrim
{
    public class PlayerInventory : Inventory
    {
        [SerializeField] private PlayerItem itemPrefab;

        private string _walletAddress;
        
        #region UNITY_LIFECYCLE
        
        private void OnDisable()
        {
            ClearAllItems();
        }

        #endregion
        

        #region PRIVATE_METHODS

        public async void LoadPurchasedOutfits()
        {
            _walletAddress = await Web3Tools.GetWalletAddress();

            try
            {
                NftOwnerCollection noc =
                    await Moralis.GetClient().Web3Api.Account.GetNFTsForContract(
                        _walletAddress.ToLower(),
                        SmartContracts.Instance.outfitContractAddress.ToLower(),
                        Moralis.CurrentChain.EnumValue);
                
                List<NftOwner> nftOwners = noc.Result;

                // We only proceed if we find some
                if (!nftOwners.Any())
                {
                    Debug.Log("You don't own any NFT Outfits");
                    return;
                }
                
                foreach (var nftOwner in nftOwners)
                {
                    if (nftOwner.Metadata == null)
                    {
                        // Sometimes GetNFTsForContract fails to get NFT Metadata. We need to re-sync
                        await Moralis.GetClient().Web3Api.Token.ReSyncMetadata(nftOwner.TokenAddress, nftOwner.TokenId, Moralis.CurrentChain.EnumValue);
                        Debug.Log("We couldn't get NFT Metadata. Re-syncing...");
                        continue;
                    }
                    
                    var nftMetaData = nftOwner.Metadata;
                    MetadataObject metadataObject = Web3Tools.DeserializeUsingNewtonSoftJson(nftMetaData);

                    PopulatePlayerItem(nftOwner.TokenId, metadataObject);
                }
            }
            catch (Exception exp)
            {
                Debug.LogError(exp.Message);
            }
        }
        
        private void PopulatePlayerItem(string tokenId, MetadataObject metadataObject)
        {
            PlayerItem newItem = Instantiate(itemPrefab, itemsGrid.transform);
            
            newItem.Init(tokenId, metadataObject);
        }

        #endregion
    }
}

