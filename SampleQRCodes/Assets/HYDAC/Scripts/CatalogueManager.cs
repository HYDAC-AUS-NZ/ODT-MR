using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;
using UnityEngine.AddressableAssets;

using HYDAC.INFO;
using HYDAC;

namespace HYDAC
{
    public class CatalogueManager : MonoBehaviour
    {
        public static CatalogueManager Instance;
        
        [SerializeField] private string catalogueLabel;

        private List<SCatalogueInfo> _catalogue = new List<SCatalogueInfo>();
        
        private void Awake()
        {
            if (Instance != null && Instance != this) 
            { 
                Destroy(this); 
            } 
            else 
            { 
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            
            StartCoroutine(DownloadCatalogue());
        }

        IEnumerator DownloadCatalogue()
        {
            // Check if assets are cached
            var downloadSizeHandle = Addressables.GetDownloadSizeAsync(catalogueLabel);
            while (!downloadSizeHandle.IsDone)
            {
                yield return null;
            }
            
            Debug.Log($"#CatalogueManager#----------Download size for catalogue: {downloadSizeHandle.Result}");
    
            // Load Scene
            var downloadCatalogueTask = Addressables.LoadAssetsAsync<SCatalogueInfo>(catalogueLabel, (result)=>
            {
                //Debug.Log("#MainManager#-------------Catalogue found: " + result.iname);
                _catalogue.Add(result);
            });
            
            while (!downloadCatalogueTask.IsDone)
            {
                if(downloadSizeHandle.Result != 0)
                {
                    var status = downloadCatalogueTask.GetDownloadStatus();
                    float progress = status.Percent; // Current download progress
                    LoadingBar.Instance.SetSliderValue(progress * 100);
                }
                yield return null;
            }
            
            //Debug.Log("#MainManager#----------Catalogue download complete");
            LoadingBar.Instance.SetSliderValue(0);
        }

        public SCatalogueInfo GetProductInfo(string productID)
        {
            var catalogue = _catalogue.FirstOrDefault(i => i.productID.Equals(productID));

            if (catalogue != null)
            {
                Debug.Log("#CatalogueManager#---------------Catalogue found");
                return catalogue;
            }

            Debug.Log("#CatalogueManager#---------------Catalogue not found");
            return null;
        }
    }
}


