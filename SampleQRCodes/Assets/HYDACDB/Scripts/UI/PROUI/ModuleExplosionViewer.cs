using System.Collections;
using System.Collections.Generic;

using UnityEngine;

using Microsoft.MixedReality.Toolkit.UI;

using HYDACDB.ADD;
using HYDACDB.INFO;
using HYDACDB.PRO;

namespace HYDACDB.UI
{
    public class ModuleExplosionViewer : BaseProductUI
    {
        // Module Viewer
        [Header("ModuleViewer Members")]
        [SerializeField] private Transform moduleSpawnPoint;
        [SerializeField] private Interactable explosionButton;
        [Range(0, 1)]
        [SerializeField] private float rotateAmount;
        private Transform _currentModuleTransform;
        private bool isRotateButtonPressed;

        [Header("SubModuleList Members")]
        [SerializeField] private Transform subModuleUIRoot;
        [SerializeField] private Canvas subModuleCanvas;
        [SerializeField] private GameObject subModuleUIBtnPrefab;
        private Transform[] _subModuleUIs = new Transform[0];


        protected override void OnModuleChanged(SModuleInfo newModule)
        {
            base.OnModuleChanged(newModule);

            // Load Submodule Model
            StartCoroutine(LoadModuleModel(newModule));

            explosionButton.gameObject.SetActive(!newModule.isStatic);
        }
        
        
        private IEnumerator LoadModuleModel(SModuleInfo newModule)
        {
            // MODEL LOADING
            //===============
            if (_currentModuleTransform != null)
                AddressableLoader.ReleaseObject(_currentModuleTransform.gameObject);
            
            var modelLoadTask = AddressableLoader.InstantiateFromReference(newModule.HighPolyReference, moduleSpawnPoint);
            yield return new WaitUntil(() => modelLoadTask.IsCompleted);
            
            _currentModuleTransform = modelLoadTask.Result.transform;
            _currentModuleTransform.localPosition = new Vector3(0, 0, 0);
            _currentModuleInfo = _currentModuleTransform.GetComponent<ProductFModule>().Info as SModuleInfo;
            
            
            // SUBMODULE LIST LOADING
            //=======================

            if (_subModuleUIs.Length > 0)
            {
                foreach (var uiButton in _subModuleUIs)
                    Destroy(uiButton.gameObject);
            
                _subModuleUIs = null;
            }

            if (newModule.isStatic)
            {
                subModuleCanvas.enabled = false;
                yield break;
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

        public void StartModelRotation()
        {
            StopAllCoroutines();
            
            isRotateButtonPressed = true;
            StartCoroutine(RotateModel());
        }

        IEnumerator RotateModel()
        {
            while (isRotateButtonPressed)
            {
                moduleSpawnPoint.Rotate(Vector3.up, rotateAmount);
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
