using UnityEngine;
using Microsoft.MixedReality.QR;

using HYDAC.INFO;
using HYDAC.QR;

namespace HYDAC.UI
{
    public class UIComponents : MonoBehaviour
    {
        [SerializeField] protected SocQRCallBacks qrCallbacks;

        protected SAssetsInfo _currentQRAssets;

        protected virtual void OnEnable()
        {
            qrCallbacks.EOnQRCodeClosed += OnQRClosed;
        }
        protected virtual void OnDisable()
        {
            qrCallbacks.EOnQRCodeClosed += OnQRClosed;
        }

        protected virtual void OnQRClosed(QRCode obj)
        {
            _currentQRAssets = null;
        }
    }
}