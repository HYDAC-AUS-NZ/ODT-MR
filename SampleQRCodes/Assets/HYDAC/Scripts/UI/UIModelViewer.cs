using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using Microsoft.MixedReality.Toolkit.UI;

using HYDAC.INFO;
using HYDAC.PRO;

namespace HYDAC.UI
{
    public class UIModelViewer : UIComponents
    {
        [Header("Module Viewer Members")]
        [SerializeField] private Transform moduleSpawnPoint;
        [SerializeField] private Interactable explosionButton;
        [SerializeField] private Interactable rotationButton;
        private Transform _currentModuleTransform;
        private bool isRotateButtonPressed;

        [Header("SubModuleList Members")]
        [SerializeField] private Transform subModuleUIRoot;
        [SerializeField] private Canvas subModuleCanvas;
        [SerializeField] private GameObject subModuleUIBtnPrefab;
        private Transform[] _subModuleUIs = new Transform[0];
        
        private SModuleInfo _currentModuleInfo;
        private AsyncOperationHandle<GameObject> _loadTaskHandle;


        protected override void OnUIComponentOpened(SAssetsInfo assetInfo)
        {
            base.OnUIComponentOpened(assetInfo);

            // Load Submodule Model
            StartCoroutine(LoadModuleModel(assetInfo));

            explosionButton.gameObject.SetActive(!assetInfo.isModelStatic);
            subModuleCanvas.enabled = !assetInfo.isModelStatic;
            rotationButton.gameObject.SetActive(true);
        }


        private IEnumerator LoadModuleModel(SAssetsInfo assetInfo)
        {
            // MODEL LOADING
            //===============
            if (_currentModuleTransform != null)
            {
                Addressables.ReleaseInstance(_currentModuleTransform.gameObject);
                Addressables.Release(_loadTaskHandle);
            }

            // Check if assets are cached
            var downloadSizeHandle = Addressables.GetDownloadSizeAsync(assetInfo.HighPolyReference);
            while (!downloadSizeHandle.IsDone)
            {
                yield return null;
            }
            
            Debug.Log($"#UIModelViewer#----------Download size for asset : {downloadSizeHandle.Result}");
            
            _loadTaskHandle = Addressables.LoadAssetAsync<GameObject>(assetInfo.HighPolyReference);
            
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
            
            var instantiateTask = Addressables.InstantiateAsync(assetInfo.HighPolyReference, moduleSpawnPoint);
            yield return new WaitUntil(() => instantiateTask.Task.IsCompleted);
            
            //Debug.Log($"#UIModelViewer#----------Asset Instantiated");
            
            _currentModuleTransform = instantiateTask.Result.transform;
            _currentModuleTransform.localPosition = new Vector3(0, 0, 0);
            _currentModuleInfo = _currentModuleTransform.GetComponent<ProductFModule>().Info as SModuleInfo;

            subModuleCanvas.enabled = !assetInfo.isModelStatic;

            if (assetInfo.isModelStatic)
            {
                yield break;
            }

            // SUBMODULE LIST LOADING
            //=======================

            if (_subModuleUIs.Length > 0)
            {
                foreach (var uiButton in _subModuleUIs)
                    Destroy(uiButton.gameObject);
            
                _subModuleUIs = null;
            }
            
            subModuleCanvas.enabled = true;
            
            var uiButtons = new List<Transform>();
            
            for (var i = 0; i < _currentModuleInfo.SubModules.Length; i++)
            {
                // Spawn submodule button
                var uiButton = Instantiate(subModuleUIBtnPrefab, subModuleUIRoot);
            
                // Get submodule button script reference and initialise it
                uiButtons.Add(uiButton.transform);
                uiButton.GetComponent<SubModuleListButton>().Intitialise(_currentModuleInfo.SubModules[i]);
            }
            
            _subModuleUIs = uiButtons.ToArray();
        }

        public void StartModelRotation(float rotateAmount)
        {
            StopAllCoroutines();
            
            isRotateButtonPressed = true;
            StartCoroutine(RotateModelContinously(rotateAmount));
        }

        public void RotateModel(float rotateAmount)
        {
            moduleSpawnPoint.Rotate(Vector3.up, rotateAmount);
        }


        IEnumerator RotateModelContinously(float rotateAmount)
        {
            while (isRotateButtonPressed)
            {
                moduleSpawnPoint.Rotate(Vector3.up, rotateAmount * Time.deltaTime);
                yield return new WaitForEndOfFrame();
            }
        }

        public void StopRotation()
        {
            isRotateButtonPressed = false;
            StopAllCoroutines();
        }
    }
}