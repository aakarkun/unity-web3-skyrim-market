using System;
using UnityEngine;

namespace Web3_Skyrim
{
    public class PlayerOutfitController : MonoBehaviour
    {
        [SerializeField] private SkinnedMeshRenderer skinnedMeshRenderer;

        // Control vars
        private Texture defaultTexture;
        
        private void Awake()
        {
            defaultTexture = skinnedMeshRenderer.material.mainTexture;
        }


        #region PUBLIC_METHODS

        public void ChangeOutfit(Texture outfitTexture)
        {
            skinnedMeshRenderer.material.mainTexture = outfitTexture;
        }

        public void DefaultOutfit()
        {
            skinnedMeshRenderer.material.mainTexture = defaultTexture;
        }

        #endregion
    }   
}
