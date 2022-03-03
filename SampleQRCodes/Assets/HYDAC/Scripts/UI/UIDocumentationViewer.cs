using System.Collections;

using UnityEngine;
using UnityEngine.AddressableAssets;

using Microsoft.MixedReality.QR;

using HYDAC.INFO;
using HYDACDB.ADD;


namespace HYDAC.UI
{
    public class UIDocumentationViewer : UIComponents
    {
        [SerializeField] private Transform canvasParent;

        private Transform _currentInfoUI;

        protected override void OnEnable()
        {
            base.OnEnable();

            qrCallbacks.EOnQRDocumentationToggle += OnDocumentationToggle;
        }

        protected override void OnDisable()
        {
            base.OnDisable();

            qrCallbacks.EOnQRDocumentationToggle -= OnDocumentationToggle;
        }

        private void OnDocumentationToggle(bool toggle, SAssetsInfo assetsInfo)
        {
            UIObject.gameObject.SetActive(toggle);

            if (toggle)
            {
                StartCoroutine(LoadDocumentation(assetsInfo.InfoUIReference));
            }
        }

        protected override void OnQRClosed(QRCode obj)
        {
            base.OnQRClosed(obj);
        }

        IEnumerator LoadDocumentation(AssetReference uiReference)
        {
            Debug.Log("#UIDocumentationViewer#------------Loading Documentation");

            // INFO UI LOADING
            //================

            if (_currentInfoUI != null)
                AddressableLoader.ReleaseObject(_currentInfoUI.gameObject);

            var infoLoadTask = AddressableLoader.InstantiateFromReference(uiReference, canvasParent);
            yield return new WaitUntil(() => infoLoadTask.IsCompleted);

            _currentInfoUI = infoLoadTask.Result.transform;
        }
    }
}