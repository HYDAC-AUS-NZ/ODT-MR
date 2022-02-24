using System.Collections.Generic;
using System.Threading.Tasks;

using UnityEngine;
using UnityEngine.AddressableAssets;

using HYDACDB.INFO;
using HYDACDB.PRO;
using UnityEngine.Serialization;

namespace HYDACDB.UI
{
    public class UICatalogue : MonoBehaviour
    {
        [SerializeField] private string catalogueLabel;

        [FormerlySerializedAs("assemblyEvents")] [SerializeField] private SocProductCallbacks productCallbacks;

        [Space] [SerializeField] private GameObject Catalogue;
        [SerializeField] private AssetReference catalogueButtonPrefab;
        [SerializeField] private Transform catalogueButtonParent;

        private Transform[] _buttonTransforms;


        private async void Awake()
        {
            // Load catalogue
            await InstantiateCatalogueUI();
            
            //Catalogue.SetActive(PlayerPrefs.GetInt("Role") == 0);
        }

        private void OnEnable()
        {
            //vproductCallbacks.EToggleCatalogueUI += OnToggleUI;
        }

        private void OnDisable()
        {
            //productCallbacks.EToggleCatalogueUI -= OnToggleUI;

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

        private void OnToggleUI(bool toggle)
        {
            //Debug.Log("Test");

            if (toggle)
            {
                transform.GetChild(0).gameObject.SetActive(true);
            }
            else
                transform.GetChild(0).gameObject.SetActive(false);
        }

        private async Task InstantiateCatalogueUI()
        {
            // Load Catalogue
            var catalogue = new List<SCatalogueInfo>();

            await Addressables.LoadAssetsAsync<SCatalogueInfo>(catalogueLabel, (result) =>
            {
                Debug.Log("#UICatalogue#-------------Catalogue found: " + result.iname);
                catalogue.Add(result);
            }).Task;

            //productCallbacks.SetCatalogue(catalogue.ToArray());

            var buttonTransforms = new List<Transform>();

            for (int i = 0; i < catalogue.Count; i++)
            {
                // Create UI button and fill in info
                var button = await Addressables.InstantiateAsync(catalogueButtonPrefab, catalogueButtonParent).Task;

                buttonTransforms.Add(button.transform);
                button.GetComponentInChildren<UIBtnCatalogue>().Initialize(catalogue[i]);
            }

            _buttonTransforms = buttonTransforms.ToArray();
        }
    }
}