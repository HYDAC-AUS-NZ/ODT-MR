using System;
using HYDAC;
using HYDACDB.INFO;
using QRTracking;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(QRTracking.SpatialGraphNodeTracker))]
public class CustomQRCode : MonoBehaviour
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

        urlText.text = qrData;
        partName.text = catalogueInfo.name;
    }
    

    public void UICloseButtonClicked()
    {
        qrCallBacks.InvokeQRCodeClosed(qrCode.qrCode);
    }
}
