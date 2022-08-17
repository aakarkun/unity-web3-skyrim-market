using System;
using TMPro;
using UnityEngine;

namespace Web3_Skyrim
{
    public class ShopItem : InventoryItem
    {
        public static event Action<ShopItem> OnSelected;

        [Header("Specific UI")]
        [SerializeField] private TextMeshProUGUI priceLabel;

        private string _metadataUrl;
        private MetadataObject _itemMetadata;
        private int _price;

        public void Init(string itemId, MetadataObject newMetadata, string metadataUrl)
        {
            SetId(itemId);

            _metadataUrl = metadataUrl;
            _itemMetadata = newMetadata;
            
            if (_itemMetadata.attributes != null)
            {
                foreach (var attribute in _itemMetadata.attributes)
                {
                    if (attribute.trait_type == "price")
                    {
                        _price = (int)attribute.value; //We assume this will be a number :)
                        priceLabel.text = _price.ToString();
                    }
                }    
            }
            else
            {
                Debug.Log("No attributes found");
                _price = 0;
            }

            StartCoroutine(GetTexture(_itemMetadata.image));
        }

        public MetadataObject GetMetadata()
        {
            return _itemMetadata;
        }

        public void SetMetadata(MetadataObject newMetadata)
        {
            _itemMetadata = newMetadata;
        }

        public int GetPrice()
        {
            return _price;
        }

        public string GetMetadataUrl()
        {
            return _metadataUrl;
        }

        public void OnClickHandler()
        {
            OnSelected?.Invoke(this);
        }
    }   
}
