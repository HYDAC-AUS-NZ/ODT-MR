using System;
using HYDAC;
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

    // Use this for initialization
    void Start()
    {
        qrCode = GetComponent<QRCode>();

        var qrData = qrCode.qrCode.Data;

        // Get Product Details
        var partID = qrData.Substring(qrData.LastIndexOf(qrDataLabel, StringComparison.Ordinal) + qrDataLabel.Length);

        urlText.text = qrData;
        partName.text = partID;
    }
    

    public void UICloseButtonClicked()
    {
        qrCallBacks.InvokeQRCodeClosed(qrCode.qrCode);
    }
}
