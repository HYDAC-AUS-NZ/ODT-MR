using System.Collections;

using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

using HYDAC.INFO;

namespace HYDAC.UI
{
    public class UIDocumentationViewer : UIComponents
    {
        [Header("Documentation Viewer Members")]
        [SerializeField] private Transform canvasParent;

        private Transform _currentInfoUI;
        private AsyncOperationHandle<GameObject> _loadTaskHandle;

        protected override void OnUIComponentOpened(SAssetsInfo assetInfo)
        {
            base.OnUIComponentOpened(assetInfo);

            StartCoroutine(LoadDocumentation(assetInfo.InfoUIReference));
        }


        IEnumerator LoadDocumentation(AssetReference uiReference)
        {
            //Debug.Log("#UIDocumentationViewer#------------Loading Documentation");

            // INFO UI LOADING
            //================
            if (_currentInfoUI != null)
            {
                Addressables.ReleaseInstance(_currentInfoUI.gameObject.gameObject);
                Addressables.Release(_loadTaskHandle);
            }

            // Check if assets are cached
            var downloadSizeHandle = Addressables.GetDownloadSizeAsync(uiReference);
            while (!downloadSizeHandle.IsDone)
            {
                yield return null;
            }
            
            Debug.Log($"#UIModelViewer#----------Download size for documentation : {downloadSizeHandle.Result}");
            
            _loadTaskHandle = Addressables.LoadAssetAsync<GameObject>(uiReference);
            
            while (!_loadTaskHandle.IsDone)
            {
                if(downloadSizeHandle.Result != 0)
                {
                    var status = _loadTaskHandle.GetDownloadStatus();
                    float progress = status.Percent; // Current download progress
                    LoadingBar.Instance.SetSliderValue(progress * 100);
                }
                yield return null;
            }

            //Debug.Log($"#UIModelViewer#----------Instantiating asset");
            
            var instantiateTask = Addressables.InstantiateAsync(uiReference, canvasParent);
            yield return new WaitUntil(() => instantiateTask.Task.IsCompleted);
            

            _currentInfoUI = instantiateTask.Result.transform;
        }
    }
}