using HYDAC.INFO;

using System.Collections;
using System.Collections.Generic;
using HYDACDB.ADD;
using HYDACDB.INFO;
using HYDACDB.PRO;
using Microsoft.MixedReality.Toolkit.UI;
using UnityEngine;

namespace HYDAC.UI
{
    public class UIModelViewer : UIComponents
    {
        // Module Viewer
        [Header("ModuleViewer Members")]
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


        protected override void OnUIComponentOpened(SAssetsInfo assetInfo)
        {
            base.OnUIComponentOpened(assetInfo);

            // Load Submodule Model
            StartCoroutine(LoadModuleModel(assetInfo));

            explosionButton.gameObject.SetActive(!assetInfo.isModelStatic);
            rotationButton.gameObject.SetActive(true);
        }


        private IEnumerator LoadModuleModel(SAssetsInfo assetInfo)
        {
            // MODEL LOADING
            //===============
            if (_currentModuleTransform != null)
                AddressableLoader.ReleaseObject(_currentModuleTransform.gameObject);
            
            var modelLoadTask = AddressableLoader.InstantiateFromReference(assetInfo.HighPolyReference, moduleSpawnPoint);
            yield return new WaitUntil(() => modelLoadTask.IsCompleted);
            
            _currentModuleTransform = modelLoadTask.Result.transform;
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