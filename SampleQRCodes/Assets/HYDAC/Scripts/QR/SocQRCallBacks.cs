using HYDAC.INFO;
using HYDAC.UI;
using System;
using UnityEngine;

using QRCode = Microsoft.MixedReality.QR.QRCode;

namespace HYDAC.QR
{
    [CreateAssetMenu(fileName = "SOC_QRCallbacks", menuName = "Socks/QRCallbacks")]
    public class SocQRCallBacks : ScriptableObject
    {
        public Action<EUIComponent> EOnUIComponentClosed;
        public void InvokeUIComponentClosed(EUIComponent component)
        {
            EOnUIComponentClosed?.Invoke(component);
        }

        public Action<QRCode> EOnQRCodeClosed;
        public void InvokeQRCodeClosed(QRCode qr)
        {
            EOnQRCodeClosed?.Invoke(qr);
        }

        public Action<EUIComponent, bool, SAssetsInfo> EOnUIComponentToggle;
        public void InvokeUIComponentToggle(EUIComponent componentType, bool toggle, SAssetsInfo assetsInfo)
        {
            EOnUIComponentToggle?.Invoke(componentType, toggle, assetsInfo);
        }
    }
}
