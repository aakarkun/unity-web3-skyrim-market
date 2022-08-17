using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;

namespace Web3_Skyrim
{
    public class MetadataObject
    {
        public string name { get; set; }
        public string description { get; set; }
        public string image { get; set; }

        [CanBeNull] public List<AttributeObject> attributes { get; set; }
    }

    public class AttributeObject
    {
        [CanBeNull] public string display_type { get; set; }
        public string trait_type { get; set; }
        public float value { get; set; }
    }
    
    public abstract class Inventory : MonoBehaviour
    {
     [Header("Base UI Elements")]
     [SerializeField] protected GridLayoutGroup itemsGrid;
     
     
     #region PRIVATE_METHODS
     
     public void ClearAllItems()
     {
         foreach (Transform childItem in itemsGrid.transform)
         {
             // We only want to destroy InventoryItems so we need to check that
             if (childItem.GetComponent<InventoryItem>())
             {
                 Destroy(childItem.gameObject);   
             }
         }
     }
     
     #endregion
     
     
     #region PUBLIC_METHODS

     public void OnCloseButtonClicked()
     {
         ClearAllItems();
     }

     #endregion
    }   
}
