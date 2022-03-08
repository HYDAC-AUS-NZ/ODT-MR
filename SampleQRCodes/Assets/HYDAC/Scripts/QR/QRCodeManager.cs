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
        [SerializeField] private Interactable closeButton;

        QRCode qrCode;
        SCatalogueInfo catalogueInfo;
        private SAssetsInfo _assetInfo;

        private void OnEnable()
        {
            modelButton.OnClick.AddListener(OnModelButtonClicked);
            documentationButton.OnClick.AddListener(OnDocumentationButtonClicked);
            videoButton.OnClick.AddListener(OnVideoButtonClicked);
            closeButton.OnClick.AddListener(OnCloseButtonClicked);

            qrCallBacks.EOnUIComponentClosed += OnUIComponentClosed;
        }

        private void OnDisable()
        {
            modelButton.OnClick.RemoveListener(OnModelButtonClicked);
            documentationButton.OnClick.RemoveListener(OnDocumentationButtonClicked);
            videoButton.OnClick.RemoveListener(OnVideoButtonClicked);
            closeButton.OnClick.RemoveListener(OnCloseButtonClicked);

            qrCallBacks.EOnUIComponentClosed -= OnUIComponentClosed;
        }

        private void OnUIComponentClosed(EUIComponent obj)
        {
            modelButton.IsToggled = false;
            documentationButton.IsToggled = false;
            videoButton.IsToggled = false;
        }


        // Use this for initialization
        void Start()
        {
            qrCode = GetComponent<QRCode>();

            AudioManager.Instance.PlayClip(AudioManager.Instance.qrScanned);

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
            _assetInfo = obj.Result;
            
            Debug.Log($"#QRCodeManager#----------Product assetInfo downloaded: {_assetInfo.name}");
            
            // Set Buttons
            modelButton.gameObject.SetActive(_assetInfo.hasModel);
            documentationButton.gameObject.SetActive(_assetInfo.hasDocumentation);
            videoButton.gameObject.SetActive(_assetInfo.hasVideo);

            modelButton.gameObject.transform.parent.GetComponent<GridObjectCollection>().UpdateCollection();
        }


        private void OnModelButtonClicked()
        {
            qrCallBacks.InvokeUIComponentToggle(UI.EUIComponent.ModelViewer, modelButton.IsToggled, _assetInfo);

            videoButton.IsToggled = false;
            documentationButton.IsToggled = false;
        }

        private void OnDocumentationButtonClicked()
        {
            qrCallBacks.InvokeUIComponentToggle(UI.EUIComponent.DocumentationViewer, documentationButton.IsToggled, _assetInfo);

            modelButton.IsToggled = false;
            videoButton.IsToggled = false;
        }

        private void OnVideoButtonClicked()
        {
            qrCallBacks.InvokeUIComponentToggle(UI.EUIComponent.VideoViewer, videoButton.IsToggled, _assetInfo);

            modelButton.IsToggled = false;
            documentationButton.IsToggled = false;
        }

        private void OnCloseButtonClicked()
        {
            qrCallBacks.InvokeQRCodeClosed(qrCode.qrCode);
        }
    }
}