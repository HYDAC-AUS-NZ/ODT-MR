using System;
using HYDAC;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(QRTracking.SpatialGraphNodeTracker))]
public class CustomQRCode : MonoBehaviour
{
    [SerializeField] private TextMeshPro partName;
    [SerializeField] private SocQRCallBacks qrCallBacks;
    
    public Microsoft.MixedReality.QR.QRCode qrCode;
    private GameObject qrCodeCube;

    public float PhysicalSize { get; private set; }
    public string CodeText { get; private set; }
    
    private bool validURI = false;
    private bool launch = false;
    private System.Uri uriResult;
    private long lastTimeStamp = 0;
    
    // Use this for initialization
    void Start()
    {
        PhysicalSize = 0.1f;
        CodeText = "Dummy";
        if (qrCode == null)
        {
            throw new System.Exception("QR Code Empty");
        }

        PhysicalSize = qrCode.PhysicalSideLength;
        CodeText = qrCode.Data;

        qrCodeCube = gameObject.transform.Find("Cube").gameObject;
        
        string qrData = qrCode.Data;
        partName.text = qrData.Substring(qrData.LastIndexOf("hydacmr", StringComparison.Ordinal) + 1);
        
        
        if (System.Uri.TryCreate(CodeText, System.UriKind.Absolute,out uriResult))
        {
            validURI = true;
        }
        
        Debug.Log("Id= " + qrCode.Id + "NodeId= " + qrCode.SpatialGraphNodeId + " PhysicalSize = " + PhysicalSize + " TimeStamp = " + qrCode.SystemRelativeLastDetectedTime.Ticks + " QRVersion = " + qrCode.Version + " QRData = " + CodeText);
    }

    void UpdatePropertiesDisplay()
    {
        // Update properties that change
        if (qrCode != null && lastTimeStamp != qrCode.SystemRelativeLastDetectedTime.Ticks)
        {
            PhysicalSize = qrCode.PhysicalSideLength;
            Debug.Log("Id= " + qrCode.Id + "NodeId= " + qrCode.SpatialGraphNodeId + " PhysicalSize = " + PhysicalSize + " TimeStamp = " + qrCode.SystemRelativeLastDetectedTime.Ticks + " Time = " + qrCode.LastDetectedTime.ToString("MM/dd/yyyy HH:mm:ss.fff"));

            qrCodeCube.transform.localPosition = new Vector3(PhysicalSize / 2.0f, PhysicalSize / 2.0f, 0.0f);
            qrCodeCube.transform.localScale = new Vector3(PhysicalSize, PhysicalSize, 0.005f);
            lastTimeStamp = qrCode.SystemRelativeLastDetectedTime.Ticks;
            
            string qrData = qrCode.Data;
            partName.text = qrData.Substring(qrData.LastIndexOf("hydacmr", StringComparison.Ordinal) + 1);
        }
    }

    // Update is called once per frame
    void Update()
    {
        UpdatePropertiesDisplay();
        if (launch)
        {
            launch = false;
            LaunchUri();
        }
    }

    void LaunchUri()
    {
#if WINDOWS_UWP
        // Launch the URI
        UnityEngine.WSA.Launcher.LaunchUri(uriResult.ToString(), true);
#endif
    }

    public void OnInputClicked()
    {
        if (validURI)
        {
            launch = true;
        }
    }


    public void UIQRButtonClicked()
    {
        qrCallBacks.InvokeQRCodeClicked(qrCode.Data);
    }
    

    public void UICloseButtonClicked()
    {
        qrCallBacks.InvokeQRCodeClosed(this.qrCode);
    }
}
