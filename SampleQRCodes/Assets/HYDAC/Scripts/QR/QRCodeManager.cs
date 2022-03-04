using System;

using UnityEngine;
using TMPro;

using QRTracking;

using HYDAC.INFO;
using HYDAC.Audio;
using Microsoft.MixedReality.Toolkit.UI;
using Microsoft.MixedReality.Toolkit.Utilities;
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
            catalogueInfo = CatalogueManager.GetProductInfo(partID);

            if (catalogueInfo == null)
            {
                Debug.LogError("#QRCodeManager#----------Product ID not found in Catalogue");
                return;
            }

            urlText.text = qrData;
            partName.text = catalogueInfo.iname;

            // Set Buttons
            var assetsInfo = catalogueInfo.AssetsInfo;
            modelButton.gameObject.SetActive(assetsInfo.hasModel);
            documentationButton.gameObject.SetActive(assetsInfo.hasDocumentation);
            videoButton.gameObject.SetActive(assetsInfo.hasVideo);

            modelButton.gameObject.transform.parent.GetComponent<GridObjectCollection>().UpdateCollection();
        }


        private void OnModelButtonClicked()
        {
            qrCallBacks.InvokeUIComponentToggle(UI.EUIComponent.ModelViewer, modelButton.IsToggled, catalogueInfo.AssetsInfo);

            videoButton.IsToggled = false;
            documentationButton.IsToggled = false;
        }

        private void OnDocumentationButtonClicked()
        {
            qrCallBacks.InvokeUIComponentToggle(UI.EUIComponent.DocumentationViewer, documentationButton.IsToggled, catalogueInfo.AssetsInfo);

            modelButton.IsToggled = false;
            videoButton.IsToggled = false;
        }

        private void OnVideoButtonClicked()
        {
            qrCallBacks.InvokeUIComponentToggle(UI.EUIComponent.VideoViewer, videoButton.IsToggled, catalogueInfo.AssetsInfo);

            modelButton.IsToggled = false;
            documentationButton.IsToggled = false;
        }

        private void OnCloseButtonClicked()
        {
            qrCallBacks.InvokeQRCodeClosed(qrCode.qrCode);
        }
    }
}