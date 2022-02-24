using System.Collections.Generic;
using System.Threading.Tasks;

using UnityEngine;
using UnityEngine.AddressableAssets;

using HYDACDB.INFO;
using HYDACDB.UI;

namespace HYDACDB
{
    public class CatalogueManager : MonoBehaviour
    {
        public static CatalogueManager Instance;
        public SCatalogueInfo CurrentSelectedProduct;
        
        [SerializeField] private string catalogueLabel;

        [Space] [SerializeField] private GameObject catalogueUIObject;
        [SerializeField] private AssetReference catalogueButtonPrefab;
        [SerializeField] private Transform catalogueButtonParent;

        private List<SCatalogueInfo> _catalogue = new List<SCatalogueInfo>();
        private Transform[] _buttonTransforms;

        #region Private Methods

        private async void Awake()
        {
            if (Instance)
            {
                Destroy(Instance);
                Destroy(Instance.gameObject);
            }

            Instance = this;

            // Load catalogue
            await InstantiateCatalogueUI();
        }


        private async Task InstantiateCatalogueUI()
        {
            await FetchCatalogue();

            await CreateCatalogueButtons();
        }

        
        private async Task CreateCatalogueButtons()
        {
            var buttonTransforms = new List<Transform>();

            for (int i = 0; i < _catalogue.Count; i++)
            {
                // Create UI button and fill in info
                var button = await Addressables.InstantiateAsync(catalogueButtonPrefab, catalogueButtonParent).Task;

                buttonTransforms.Add(button.transform);
                button.GetComponentInChildren<UIBtnCatalogue>().Initialize(_catalogue[i]);
            }

            _buttonTransforms = buttonTransforms.ToArray();
        }

        
        private async Task FetchCatalogue()
        {
            // Load Catalogue
            _catalogue = new List<SCatalogueInfo>();

            await Addressables.LoadAssetsAsync<SCatalogueInfo>(catalogueLabel, (result) =>
            {
                Debug.Log("#UICatalogue#-------------Catalogue found: " + result.iname);
                _catalogue.Add(result);
            }).Task;
        }
        
        
        private void EmptyFetchedCatalogue()
        {
            for (int i = 0; i < _buttonTransforms.Length; i++)
            {
                bool check = Addressables.ReleaseInstance(_buttonTransforms[i].gameObject);
                if (!check)
                {
                    Debug.LogError("#UICatalogue#---------Releasing button failed: " + _buttonTransforms[i].name);
                }
            }

            _buttonTransforms = null;
        }
        
        #endregion

        public SCatalogueInfo GetProductFromCatalogue(int productID)
        {
            foreach (var catalogueEntry in _catalogue)
            {
                if (catalogueEntry.ID == productID)
                {
                    CurrentSelectedProduct = catalogueEntry;
                    return catalogueEntry;
                }
            }

            return null;
        }
        
        public void OnToggleUI(bool toggle)
        {
            if (toggle)
            {
                catalogueUIObject.SetActive(true);
                if(_buttonTransforms == null)
                    InstantiateCatalogueUI();
            }
            else
            {
                catalogueUIObject.SetActive(false);
                if(_buttonTransforms != null)
                    EmptyFetchedCatalogue();
            }
        }
    }
}