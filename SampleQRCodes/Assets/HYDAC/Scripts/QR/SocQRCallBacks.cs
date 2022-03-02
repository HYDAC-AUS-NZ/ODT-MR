using System;
using UnityEngine;

using QRCode = Microsoft.MixedReality.QR.QRCode;

namespace HYDAC.QR
{
    [CreateAssetMenu(fileName = "SOC_QRCallbacks", menuName = "Socks/QRCallbacks")]
    public class SocQRCallBacks : ScriptableObject
    {
        public Action<QRCode> EOnQRCodeClosed;

        public void InvokeQRCodeClosed(QRCode qr)
        {
            EOnQRCodeClosed?.Invoke(qr);
        }
    
        public Action<string> EOnQRCodeClicked;

        public void InvokeQRCodeClicked(string qrData)
        {
            EOnQRCodeClicked?.Invoke(qrData);
        }
    }
}
