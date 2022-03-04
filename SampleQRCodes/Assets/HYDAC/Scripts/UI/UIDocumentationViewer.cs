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

        protected override void OnUIComponentOpened(SAssetsInfo assetInfo)
        {
            base.OnUIComponentOpened(assetInfo);

            StartCoroutine(LoadDocumentation(assetInfo.InfoUIReference));
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