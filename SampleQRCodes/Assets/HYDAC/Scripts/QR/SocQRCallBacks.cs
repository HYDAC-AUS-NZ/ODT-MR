using HYDAC.INFO;
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
    

        public Action<bool, SAssetsInfo> EOnQRModelToggle;
        public void InvokeUIQRModelToggle(bool toggle, SAssetsInfo assetsInfo)
        {
            EOnQRModelToggle?.Invoke(toggle, assetsInfo);
        }


        public Action<bool, SAssetsInfo> EOnQRDocumentationToggle;
        public void InvokeUIQRDocumentationToggle(bool toggle, SAssetsInfo assetsInfo)
        {
            EOnQRDocumentationToggle?.Invoke(toggle, assetsInfo);
        }


        public Action<bool, SAssetsInfo> EOnQRVideoToggle;
        public void InvokeUIQRVideoToggle(bool toggle, SAssetsInfo assetsInfo)
        {
            EOnQRVideoToggle?.Invoke(toggle, assetsInfo);
        }
    }
}
