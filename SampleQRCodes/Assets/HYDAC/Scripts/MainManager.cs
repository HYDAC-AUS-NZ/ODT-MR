using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using UnityEngine;
using UnityEngine.AddressableAssets;

using HYDAC.INFO;
using QRTracking;

namespace HYDAC
{
    public static class CatalogueManager
    {
        private static List<SCatalogueInfo> _catalogue = new List<SCatalogueInfo>();

        public static async Task FetchCatalogue(string catalogueLabel)
        {
            // Load Catalogue
            _catalogue = new List<SCatalogueInfo>();

            await Addressables.LoadAssetsAsync<SCatalogueInfo>(catalogueLabel, (result) =>
            {
                Debug.Log("#CatalogueManager#-------------Catalogue found: " + result.iname);
                _catalogue.Add(result);
            }).Task;
        }

        public static SCatalogueInfo[] GetFetchedCatalogue()
        {
            if (_catalogue.Count > 0)
            {
                return _catalogue.ToArray();
            }

            return null;
        }

        public static SCatalogueInfo GetProductInfo(string productID)
        {
            var catalogue = _catalogue.Where(i => i.productID.Equals(productID)).FirstOrDefault();

            if (catalogue != null)
            {
                Debug.Log("#CatalogueManager#---------------Catalogue found");
                return catalogue;
            }

            Debug.Log("#CatalogueManager#---------------Catalogue not found");
            return null;
        }
    }


    public class MainManager : MonoBehaviour
    {
        [SerializeField] private string catalogueLabel;

        private async void Start()
        {
            await CatalogueManager.FetchCatalogue(catalogueLabel);

            Debug.Log("#MainManager#-------------Starting QR Scanning");

            QRCodesManager.Instance.StartQRTracking();
        }
    }
}


