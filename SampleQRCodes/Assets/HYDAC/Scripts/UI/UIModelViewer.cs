using HYDAC.INFO;
using Microsoft.MixedReality.QR;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HYDAC.UI
{
    public class UIModelViewer : UIComponents
    {

        protected override void OnEnable()
        {
            base.OnEnable();
            qrCallbacks.EOnQRModelToggle += OnModelToggle;
        }


        protected override void OnDisable()
        {
            base.OnDisable();
            qrCallbacks.EOnQRModelToggle += OnModelToggle;
        }

        private void OnModelToggle(bool toggle, SAssetsInfo assetsInfo)
        {
            if (toggle)
            {
                UIObject.gameObject.SetActive(true);
            }
        }

        protected override void OnQRClosed(QRCode obj)
        {
            base.OnQRClosed(obj);
        }
    }
}