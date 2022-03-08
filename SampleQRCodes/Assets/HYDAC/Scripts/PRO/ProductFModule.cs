using System.Collections;
using System.Collections.Generic;
using HYDAC.INFO;
using UnityEngine;

namespace HYDAC.PRO
{
    public class ProductFModule : AProduct
    {
        [SerializeField] private SocProductCallbacks productCallbacks;
        [SerializeField] private Transform rootTransform;
        [SerializeField] private Transform explodedRootTransform;

        [Header("Debug")]
        [SerializeField] private int _subModulesCount;
        [SerializeField] private ProductSubModule[] _subModules;
        [SerializeField] private Vector3[] _disassembledPositions;
        [SerializeField] private Vector3[] _assembledPositions;

        [Space]
        [SerializeField] private bool _isAssembled = true;
        [SerializeField] private bool _isBusy = false;


#if UNITY_EDITOR
        // For Editor Script
        public Transform RootTransform => rootTransform;
        public ProductSubModule[] SubModules => _subModules;
        public int SubModulesCount => _subModulesCount;
#endif

        private void Start()
        {
            //BoundsControl boundsControl = GetComponentInParent<BoundsControl>();
            //boundsControl.BoundsOverride = GetComponent<BoxCollider>();

            UpdateSubModules();
        }


        private void OnEnable()
        {
            productCallbacks.EModuleExplode += OnExplosionToggleRequest;
            productCallbacks.ESubModuleSelected += OnSubModuleSelected;
        }


        private void OnDisable()
        {
            productCallbacks.EModuleExplode -= OnExplosionToggleRequest;
            productCallbacks.ESubModuleSelected -= OnSubModuleSelected;
        }

        public void ToggleExplosion(bool toggle)
        {
            OnExplosionToggleRequest(toggle);
        }


        private void OnExplosionToggleRequest(bool toggle)
        {
            if (((SModuleInfo) info).isStatic) return;

            StopAllCoroutines();
            StartCoroutine(ToggleExplosion(toggle, -1));

            _isBusy = true;
        }


        IEnumerator ToggleExplosion(bool toggle, int subMododuleID = -1)
        {
            var t = 0f;

            while (t < 1)
            {
                t += Time.deltaTime / productCallbacks.ExplodeTime;

                for (var i = 0; i < _subModulesCount; i++)
                {
                    var currentTransform = rootTransform.GetChild(i);
                    var currentPos = currentTransform.localPosition;

                    Vector3 endPos = Vector3.zero;

                    // If there was no sub module selected then toggle explosion
                    if(subMododuleID == -1)
                    {
                        endPos = (toggle) ? _disassembledPositions[i] : _assembledPositions[i];
                        _subModules[i].ToggleMeshes(true);
                    }
                    // If a submodule was selected then explode only the selected module
                    //else
                    //{
                    //    endPos = (_subModules[i].Info.ID == subMododuleID) ? _disassembledPositions[i] : _assembledPositions[i];
                    //}

                    // Lerp
                    currentTransform.localPosition = Vector3.Lerp(currentPos, endPos, t);
                }

                yield return null;
            }

            _isAssembled = toggle;
            _isBusy = false;

            yield return null;
        }



        private void OnSubModuleSelected(SSubModuleInfo selectedSubModule)
        {
            if (_isBusy || ((SModuleInfo) info).isStatic) return;
            //StartCoroutine(ToggleExplosion(false, selectedSubModule.ID));

            // Disable all other meshes except the selected mesh
            for (var i = 0; i < _subModulesCount; i++)
            {
                _subModules[i].ToggleMeshes(_subModules[i].Info.Equals(selectedSubModule) ? true : false);
            }
        }


        /// <summary>
        /// Get all the submodules and their exploded/imploded transforms
        /// </summary>
        /// <returns></returns>
        public bool UpdateSubModules()
        {
            if (((SModuleInfo) info).isStatic) return false;

            // Get the count of sub modules
            _subModulesCount = rootTransform.childCount;

            List<Vector3> implodedPos = new List<Vector3>();
            List<Vector3> explodedPos = new List<Vector3>();

            List<ProductSubModule> subModules = new List<ProductSubModule>();

            // Ensure each sub module has an exploded transform
            if (explodedRootTransform.childCount != _subModulesCount)
            {
                Debug.LogError("#FocusedModule#--------Error: HIERARCHY - Children count not same");
                return false;
            }

            // Iterate through each submodule
            for (int i = 0; i < explodedRootTransform.childCount; i++)
            {
                var rootChild = rootTransform.GetChild(i);
                var explodedRootChild = explodedRootTransform.GetChild(i);

                // Add to list only if the disassembled transform has the same name as its counter part
                if (explodedRootChild.name.Contains(rootChild.name))
                {
                    explodedPos.Add(explodedRootChild.localPosition);
                    implodedPos.Add(rootChild.localPosition);
                }
                else
                {
                    Debug.LogError("#FocusedModule#--------Error: HIERARCHY - Check children names");
                    return false;
                }


                // Check if child already has component
                rootChild.TryGetComponent<ProductSubModule>(out ProductSubModule subModule);
                if (subModule == null)
                {
                    // Add submodule component
                    subModule = rootChild.gameObject.AddComponent<ProductSubModule>();
                }

                subModules.Add(subModule);
            }

            _assembledPositions = implodedPos.ToArray();
            _disassembledPositions = explodedPos.ToArray();

            _subModules = subModules.ToArray();

            return true;
        }
    }
}