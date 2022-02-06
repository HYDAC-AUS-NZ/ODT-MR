using System.Collections;
using UnityEngine;

using HYDACDB.ADD;
using HYDACDB.INFO;

namespace HYDACDB.UI
{
    public class ModuleDocumentationViewer : BaseProductUI
    {
        [Header("Documentation Viewer Memebers")]
        [SerializeField] private Transform moduleInfoSpawnPoint;
        private Transform _currentInfoUI;


        protected override void OnModuleChanged(SModuleInfo newModule)
        {
            base.OnModuleChanged(newModule);

            StopAllCoroutines();
            StartCoroutine(LoadDocumentation(newModule));
        }

        IEnumerator LoadDocumentation(SModuleInfo newModule)
        {
            // INFO UI LOADING
            //================

            if (_currentInfoUI != null)
                AddressableLoader.ReleaseObject(_currentInfoUI.gameObject);

            var infoLoadTask = AddressableLoader.InstantiateFromReference(newModule.InfoUIReference, moduleInfoSpawnPoint);
            yield return new WaitUntil(() => infoLoadTask.IsCompleted);

            _currentInfoUI = infoLoadTask.Result.transform;
        }
    }
}