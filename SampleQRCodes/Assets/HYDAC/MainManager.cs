using System.Collections.Generic;
using System.Threading.Tasks;
using HYDACDB;
using HYDACDB.ADD;
using HYDACDB.INFO;
using HYDACDB.PRO;
using QRTracking;

using UnityEngine;
using UnityEngine.ResourceManagement.ResourceLocations;
using QRCode = Microsoft.MixedReality.QR.QRCode;

public class MainManager : MonoBehaviour
{
    [SerializeField] private SocProductCallbacks productCallbacks;
    [SerializeField] private QRCodesManager _qrCodesManager;

    private IList<IResourceLocation> _productAssetsLocations = new List<IResourceLocation>();
    
    private void OnEnable()
    {
        productCallbacks.EAssemblySelected += OnProductSelected;
        
        _qrCodesManager.QRCodeUpdated += OnQRCodeUpdated;
        _qrCodesManager.QRCodeAdded += OnQRCodeAdded;
        _qrCodesManager.QRCodesTrackingStateChanged += OnQRCodesTrackingStateChanged;
        _qrCodesManager.QRCodeRemoved += OnQRCodeRemoved;
    }

    private void OnDisable()
    {
        productCallbacks.EAssemblySelected += OnProductSelected;
        
        _qrCodesManager.QRCodeUpdated -= OnQRCodeUpdated;
        _qrCodesManager.QRCodeAdded -= OnQRCodeAdded;
        _qrCodesManager.QRCodesTrackingStateChanged -= OnQRCodesTrackingStateChanged;
        _qrCodesManager.QRCodeRemoved -= OnQRCodeRemoved;
    }
    
    
    private void OnQRCodeRemoved(object sender, QRCodeEventArgs<QRCode> e)
    {
        Debug.Log($"#MainManager#-------------QR Removed: {e.Data.Data}");
    }

    private void OnQRCodesTrackingStateChanged(object sender, bool e)
    {
        Debug.Log($"#MainManager#-------------QRCodesTrackingStateChanged: {e}");
    }

    private void OnQRCodeAdded(object sender, QRCodeEventArgs<QRCode> e)
    {
        Debug.Log($"#MainManager#-------------QR Added: {e.Data.Data}");
    }

    private void OnQRCodeUpdated(object sender, QRCodeEventArgs<QRCode> e)
    {
        Debug.Log($"#MainManager#-------------QR Updated: {e.Data.Data}");
    }



    private void OnProductSelected(SCatalogueInfo info)
    {
        Debug.Log($"#MainManager#-------------Product Selected");
        StartProcess(info);
    }


    private async void StartProcess(SCatalogueInfo info)
    {
        Debug.Log($"#MainManager#-------------Process Started");

        await LoadProductAssets(info);
        
        Debug.Log($"#MainManager#-------------Completed Loading Assets");

        QRCodesManager.Instance.StartQRTracking();
    }
    
    
    private async Task LoadProductAssets(SCatalogueInfo catalogueInfo)
    {
        LoadingBar.Instance.StartLoading($"Loading {catalogueInfo.iname} assets..Please wait");

        // Load product dependencies
        _productAssetsLocations = await AddressableLoader.LoadLabels(new string[] { catalogueInfo.ProductAssetsKey });

        productCallbacks.OnProductSelected(catalogueInfo);

        // // Get Modules of Assembly
        // var productModules = _currentProduct.GetProductModules();
        //
        // SModuleInfo[] moduleInfos = new SModuleInfo[productModules.Length];
        //
        // for (int i = 0; i < productModules.Length; i++)
        // {
        //     // Register for the module OnClick event
        //     productModules[i].EOnClicked += OnModuleSelect;
        //
        //     moduleInfos[i] = (SModuleInfo)productModules[i].Info;
        //
        //     // Set module infos in AssemblyEvents sock
        //     productCallbacks.Modules = moduleInfos;
        // }
            
        LoadingBar.Instance.StopLoading();
    }
}
