using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

namespace Web3_Skyrim
{
    public abstract class InventoryItem : MonoBehaviour
    {
        private string _itemId;
        private Texture _rawTexture;
        
        [Header("Base UI Elements")]
        [SerializeField] protected Image myIcon;
        [SerializeField] protected Button myButton;

        private UnityWebRequest _currentWebRequest;

        
        #region UNITY_LIFECYCLE

        protected void OnEnable()
        {
            //We will activate them when the texture is retrieved
            myIcon.gameObject.SetActive(false);
            myButton.interactable = false;
        }

        private void OnDisable()
        {
            StopAllCoroutines();
            _currentWebRequest?.Dispose();
        }

        #endregion


        #region PUBLIC_METHODS

        public Sprite GetSprite()
        {
            return myIcon.sprite;
        }
        
        public Texture GetTexture()
        {
            return _rawTexture;
        }

        public string GetId()
        {
            return _itemId;
        }

        protected void SetId(string newId)
        {
            _itemId = newId;
        }

        #endregion


        #region PROTECTED_METHODS

        protected IEnumerator GetTexture(string imageUrl)
        {
            using UnityWebRequest uwr = UnityWebRequestTexture.GetTexture(imageUrl);
            _currentWebRequest = uwr;
            
            yield return uwr.SendWebRequest();

            if (uwr.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(uwr.error);
                uwr.Dispose();
            }
            else
            {
                // We save the raw texture, maybe we need it :)
                var tex = DownloadHandlerTexture.GetContent(uwr);
                _rawTexture = tex;
                
                myIcon.sprite = Sprite.Create(tex, new Rect(0.0f, 0.0f, tex.width, tex.height), new Vector2(0.5f, 0.5f), 100.0f);
                    
                //Now we are able to click the button and we will pass the loaded sprite :)
                myIcon.gameObject.SetActive(true);
                myButton.interactable = true;
                
                uwr.Dispose();
            }
        }

        #endregion
    }    
}

