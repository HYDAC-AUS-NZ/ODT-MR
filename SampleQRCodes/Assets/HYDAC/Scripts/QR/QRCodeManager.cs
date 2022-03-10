using System;

using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

using Microsoft.MixedReality.Toolkit.UI;
using Microsoft.MixedReality.Toolkit.Utilities;

using TMPro;

using QRTracking;

using HYDAC.INFO;
using HYDAC.Audio;
using HYDAC.UI;

namespace HYDAC.QR
{ 
    [RequireComponent(typeof(QRTracking.SpatialGraphNodeTracker))]
    public class QRCodeManager : MonoBehaviour
    {
        [SerializeField] private SocQRCallBacks qrCallBacks;
        [Space]
        [SerializeField] private string qrDataLabel;
        [Space]
        [SerializeField] private TextMeshPro partName;
        [SerializeField] private TextMeshPro urlText;
        [Space]
        [SerializeField] private Interactable modelButton;
        [SerializeField] private Interactable documentationButton;
        [SerializeField] private Interactable videoButton;
        [SerializeField] private Interactable schematicButton;
        [SerializeField] private Interactable closeButton;

        QRCode qrCode;
        SCatalogueInfo catalogueInfo;
        private SAssetsInfo _assetInfo;

        // Use this for initialization
        void Start()
        {
            qrCallBacks.EOnUIComponentClosed += OnUIComponentClosed;

            qrCode = GetComponent<QRCode>();

            SetupQRCode();
        }

        private void SetupQRCode()
        {
            // Get Product Details
            var qrData = qrCode.qrCode.Data;
            var partID = qrData.Substring(qrData.LastIndexOf(qrDataLabel, StringComparison.Ordinal) + qrDataLabel.Length);
            catalogueInfo = CatalogueManager.Instance.GetProductInfo(partID);

            if (catalogueInfo == null)
            {
                Debug.LogError("#QRCodeManager#----------Product ID not found in Catalogue");
                return;
            }

            urlText.text = qrData;
            partName.text = catalogueInfo.iname;

            var handle = Addressables.LoadAssetAsync<SAssetsInfo>(catalogueInfo.AssetsInfo);
            handle.Completed += OnAssetInfoLoadingComplete;
        }

        private void OnAssetInfoLoadingComplete(AsyncOperationHandle<SAssetsInfo> obj)
        {
            AudioManager.Instance.PlayClip(AudioManager.Instance.qrScanned);

            qrCallBacks.InvokeQRCodeCreated(qrCode.qrCode);

            _assetInfo = obj.Result;
            
            Debug.Log($"#QRCodeManager#----------Product assetInfo downloaded: {_assetInfo.name}");
            
            // Setup Buttons

            closeButton.OnClick.AddListener(OnCloseButtonClicked);

            modelButton.gameObject.SetActive(_assetInfo.hasModel);
            documentationButton.gameObject.SetActive(_assetInfo.hasDocumentation);
            videoButton.gameObject.SetActive(_assetInfo.hasVideo);
            schematicButton.gameObject.SetActive(_assetInfo.hasSchematic);
            
            modelButton.gameObject.transform.parent.GetComponent<GridObjectCollection>().UpdateCollection();

            if (_assetInfo.hasModel)
            {
                modelButton.OnClick.AddListener(OnModelButtonClicked);
                modelButton.gameObject.SetActive(true);
            }
            if (_assetInfo.hasDocumentation)
            {
                documentationButton.OnClick.AddListener(OnDocumentationButtonClicked);
                documentationButton.gameObject.SetActive(true);
            }
            if (_assetInfo.hasVideo)
            {
                videoButton.OnClick.AddListener(OnVideoButtonClicked);
                videoButton.gameObject.SetActive(true);
            }
            if (_assetInfo.hasSchematic)
            {
                schematicButton.OnClick.AddListener(OnSchematicButtonClicked);
                schematicButton.gameObject.SetActive(true);
            }
        }
        
        private void OnUIComponentClosed(EUIComponent obj)
        {
            modelButton.IsToggled = false;
            documentationButton.IsToggled = false;
            videoButton.IsToggled = false;
            schematicButton.IsToggled = false;
        }

        private void OnModelButtonClicked()
        {
            qrCallBacks.InvokeUIComponentToggle(UI.EUIComponent.ModelViewer, modelButton.IsToggled, _assetInfo);

            videoButton.IsToggled = false;
            documentationButton.IsToggled = false;
            schematicButton.IsToggled = false;
        }

        private void OnDocumentationButtonClicked()
        {
            qrCallBacks.InvokeUIComponentToggle(UI.EUIComponent.DocumentationViewer, documentationButton.IsToggled, _assetInfo);

            modelButton.IsToggled = false;
            videoButton.IsToggled = false;
            schematicButton.IsToggled = false;
        }

        private void OnVideoButtonClicked()
        {
            qrCallBacks.InvokeUIComponentToggle(UI.EUIComponent.VideoViewer, videoButton.IsToggled, _assetInfo);

            modelButton.IsToggled = false;
            documentationButton.IsToggled = false;
            schematicButton.IsToggled = false;
        }
        
        private void OnSchematicButtonClicked()
        {
            qrCallBacks.InvokeUIComponentToggle(UI.EUIComponent.SchematicViewer, schematicButton.IsToggled, _assetInfo);
            
            schematicButton.IsToggled = true;

            modelButton.IsToggled = false;
            documentationButton.IsToggled = false;
            videoButton.IsToggled = false;
        }

        private void OnCloseButtonClicked()
        {
            qrCallBacks.InvokeQRCodeClosed(qrCode.qrCode);
        }
        
        private void OnDestroy()
        {
            qrCallBacks.EOnUIComponentClosed -= OnUIComponentClosed;
            
            closeButton.OnClick.RemoveListener(OnCloseButtonClicked);

            if (_assetInfo.hasModel)
            {
                modelButton.OnClick.RemoveListener(OnModelButtonClicked);
            }
            if (_assetInfo.hasDocumentation)
            {
                documentationButton.OnClick.RemoveListener(OnDocumentationButtonClicked);
            }
            if (_assetInfo.hasVideo)
            {
                videoButton.OnClick.RemoveListener(OnVideoButtonClicked);
            }
            if (_assetInfo.hasSchematic)
            {
                schematicButton.OnClick.RemoveListener(OnSchematicButtonClicked);
            }
        }
    }
}