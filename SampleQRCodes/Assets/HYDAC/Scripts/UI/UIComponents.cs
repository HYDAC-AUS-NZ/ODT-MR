using UnityEngine;
using Microsoft.MixedReality.QR;

using HYDAC.INFO;
using HYDAC.QR;
using Microsoft.MixedReality.Toolkit.UI;
using System;
using Microsoft.MixedReality.Toolkit.Utilities.Solvers;

namespace HYDAC.UI
{
    [Serializable]
    public enum EUIComponent
    {
        ModelViewer,
        DocumentationViewer,
        VideoViewer,
        SchematicViewer
    }


    public class UIComponents : MonoBehaviour
    {
        [SerializeField] protected EUIComponent componentType;
        [SerializeField] protected SocQRCallBacks qrCallbacks;
        [SerializeField] protected Transform UIObject;
        [SerializeField] protected Interactable closeButton;
        [SerializeField] protected Interactable pinButton;

        protected SAssetsInfo _currentQRAssets;

        protected virtual void OnEnable()
        {
            closeButton.OnClick.AddListener(OnUICloseButtonPressed);

            qrCallbacks.EOnUIComponentToggle += OnUIComponentToggle;
            qrCallbacks.EOnQRCodeClosed += OnQRClosed;
        }
        protected virtual void OnDisable()
        {
            closeButton.OnClick.RemoveListener(OnUICloseButtonPressed);

            qrCallbacks.EOnUIComponentToggle -= OnUIComponentToggle;
            qrCallbacks.EOnQRCodeClosed -= OnQRClosed;
        }

        protected virtual void OnUIComponentToggle(EUIComponent component, bool toggle, SAssetsInfo assetInfo = null)
        {
            // If it is the current component
            if (component.Equals(componentType))
            {
                if (toggle)
                {
                    UIObject.gameObject.SetActive(true);
                    UIObject.GetComponent<FollowMeToggle>().SetFollowMeBehavior(true);
                    pinButton.IsToggled = false;

                    this._currentQRAssets = assetInfo;
                    OnUIComponentOpened(this._currentQRAssets);

                    return;
                }
                else
                {
                    UIObject.gameObject.SetActive(false);
                    this._currentQRAssets = null;
                    OnUIComponentClosed();

                    return;
                }
            }
            // If it not the current UIComponent
            else
            {
                // Disable this component if another one is toggled on
                if (toggle)
                {
                    UIObject.gameObject.SetActive(false);
                    this._currentQRAssets = assetInfo;
                    OnUIComponentClosed();

                    return;
                }
            }
        }

        protected virtual void OnUICloseButtonPressed()
        {
            qrCallbacks.InvokeUIComponentClosed(this.componentType);
        }


        // Close all components if the QR is closed
        protected virtual void OnQRClosed(QRCode qrCode)
        {
            this._currentQRAssets = null;
            OnUIComponentToggle(this.componentType, false);
        }

        protected virtual void OnUIComponentOpened(SAssetsInfo assetInfo) { }
        protected virtual void OnUIComponentClosed() { }

    }
}