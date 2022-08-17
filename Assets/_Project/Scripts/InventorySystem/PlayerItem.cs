using System;

namespace Web3_Skyrim
{
    public class PlayerItem : InventoryItem
    {
        public static event Action<PlayerItem> OnSelected;
        
        public void Init(string tokenId, MetadataObject metadataObject)
        {
            SetId(tokenId);
            StartCoroutine(GetTexture(metadataObject.image));
        }
        
        public void OnClickHandler()
        {
            OnSelected?.Invoke(this);
        }
    }   
}
