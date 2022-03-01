using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HYDACDB;
using HYDACDB.ADD;
using HYDACDB.INFO;
using HYDACDB.PRO;
using QRTracking;

using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.ResourceLocations;
using QRCode = Microsoft.MixedReality.QR.QRCode;

public class MainManager : MonoBehaviour
{
    [SerializeField] private SocProductCallbacks productCallbacks;
    [SerializeField] private string catalogueLabel;

    private IList<IResourceLocation> _productAssetsLocations = new List<IResourceLocation>();
    private List<SCatalogueInfo> _catalogue = new List<SCatalogueInfo>();


    private void OnEnable()
    {
        //productCallbacks.EAssemblySelected += OnProductSelected;
    }

    private void OnDisable()
    {
        //productCallbacks.EAssemblySelected += OnProductSelected;
    }


    private async void Start()
    {
        await CatalogueManager.FetchCatalogue(catalogueLabel);

        Debug.Log("#MainManager#-------------Starting QR Scanning");

        QRCodesManager.Instance.StartQRTracking();
    }
}

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
        return _catalogue.Where(i => i.productID.Equals(productID)).FirstOrDefault();
    }
}
