using System;

using UnityEngine;
using TMPro;

using QRTracking;

using HYDAC.INFO;

namespace HYDAC.QR
{ 
    [RequireComponent(typeof(QRTracking.SpatialGraphNodeTracker))]
    public class QRCodeManager : MonoBehaviour
    {
        [SerializeField] private SocQRCallBacks qrCallBacks;
        [Space]
        [SerializeField] private TextMeshPro partName;
        [SerializeField] private TextMeshPro urlText;
        [Space]
        [SerializeField] private string qrDataLabel;

        QRCode qrCode;
        SCatalogueInfo catalogueInfo;

        // Use this for initialization
        void Start()
        {
            qrCode = GetComponent<QRCode>();

            var qrData = qrCode.qrCode.Data;

            // Get Product Details
            var partID = qrData.Substring(qrData.LastIndexOf(qrDataLabel, StringComparison.Ordinal) + qrDataLabel.Length);

            catalogueInfo = CatalogueManager.GetProductInfo(partID);

            if(catalogueInfo != null)
            {
                urlText.text = qrData;
                partName.text = catalogueInfo.iname;
            }
        }
    

        public void UICloseButtonClicked()
        {
            qrCallBacks.InvokeQRCodeClosed(qrCode.qrCode);
        }
    }
}