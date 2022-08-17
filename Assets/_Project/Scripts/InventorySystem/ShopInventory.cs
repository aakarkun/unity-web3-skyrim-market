using System.Collections.Generic;
using UnityEngine;

namespace Web3_Skyrim
{
    public class ShopInventory : Inventory
    {
        [HideInInspector] public List<ShopItem> currentItems = new List<ShopItem>();
        
        [Header("Shop Item Prefab")]
        [SerializeField] private ShopItem itemPrefab;

        public void PopulateShopItem(string itemId, MetadataObject itemMetadata, string metadataUrl)
        {
            ShopItem newItem = Instantiate(itemPrefab, itemsGrid.transform);
    
            newItem.Init(itemId, itemMetadata, metadataUrl);
            currentItems.Add(newItem);
        }
        
        // Not used in this project
        public void UpdateItemMetadata(string idToUpdate, MetadataObject itemMetadata)
        {
            foreach (Transform childItem in itemsGrid.transform)
            {
                ShopItem item = childItem.GetComponent<ShopItem>();

                if (item.GetId() == idToUpdate)
                {
                    item.SetMetadata(itemMetadata);
                }
            }
        }
        
        // Not used in this project
        public void DeleteItem(string idToDelete)
        {
            foreach (Transform childItem in itemsGrid.transform)
            {
                ShopItem item = childItem.GetComponent<ShopItem>();

                if (item.GetId() == idToDelete)
                {
                    Destroy(item.gameObject);
                }
            }
        }
    }   
}
