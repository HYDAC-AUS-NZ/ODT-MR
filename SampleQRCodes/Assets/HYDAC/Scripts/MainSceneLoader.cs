using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.MixedReality.Toolkit;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.AddressableAssets.ResourceLocators;
using UnityEngine.Android;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

namespace HYDAC
{
    public class MainSceneLoader : MonoBehaviour
    {
        [SerializeField] private List<AssetReference> sceneReferences = new List<AssetReference>();
        [SerializeField] private string activeSceneName;
        
        private List<AsyncOperationHandle<SceneInstance>> _sceneHandles = new List<AsyncOperationHandle<SceneInstance>>();
        private int _sceneLoadCount = 0;
        
        private void Awake()
        {
            DontDestroyOnLoad(gameObject);

// #if UNITY_EDITOR
//             PlayerPrefs.DeleteAll();
// #endif

            // Initialise Addressables
            Addressables.InitializeAsync();
            Addressables.InitializeAsync().Completed += OnAddressablesInitialised;
            
            if (CoreServices.InputSystem?.InputSystemProfile != null)
            {
                var handProfile = CoreServices.InputSystem.InputSystemProfile.HandTrackingProfile;
                handProfile.EnableHandJointVisualization = false;
            }
        }

        private void OnAddressablesInitialised(AsyncOperationHandle<IResourceLocator> handle)
        {
            if (handle.Status == AsyncOperationStatus.Succeeded)
            {
                StartCoroutine(DownloadScenes());
            }
        }
        
        IEnumerator DownloadScenes()
        {
            for(int i = 0; i < sceneReferences.Count; i++)
            {
                LoadingBar.Instance.StartLoading("Loading please wait...");

                var sceneToBeLoaded = sceneReferences[i];
                StartCoroutine(DownloadScene(sceneToBeLoaded));

                while (_sceneLoadCount != i + 1)
                {
                    yield return null;
                }
            }
            
            LoadingBar.Instance.StopLoading();
        }
        

        IEnumerator DownloadScene(AssetReference sceneToBeLoaded)
        {
            // Check if assets are cached
            var downloadSizeHandle = Addressables.GetDownloadSizeAsync(sceneToBeLoaded);
            while (!downloadSizeHandle.IsDone)
            {
                yield return null;
            }
            
            Debug.Log($"#MainSceneLoader#----------Download size for scene: {downloadSizeHandle.Result}");
    
            // Load Scene
            var downloadScene = Addressables.LoadSceneAsync(sceneToBeLoaded, LoadSceneMode.Additive);
            downloadScene.Completed += SceneDownloadComplete;

            //Debug.Log($"#MainSceneLoader#----------Starting scene download: {_sceneLoadCount + 1} ");
        
            while (!downloadScene.IsDone)
            {
                if(downloadSizeHandle.Result != 0)
                {
                    var status = downloadScene.GetDownloadStatus();
                    float progress = status.Percent; // Current download progress
                    LoadingBar.Instance.SetSliderValue(progress * 100);
                }
                yield return null;
            }
            
            //Debug.Log("Download complete, starting next scene");
            LoadingBar.Instance.SetSliderValue(0);
        }
        
        
        private void SceneDownloadComplete(AsyncOperationHandle<SceneInstance> handle)
        {
            if (handle.Status == AsyncOperationStatus.Succeeded)
            {
                var scene = handle.Result.Scene;
                
                if (scene.name.Equals(activeSceneName))
                {
                    //Debug.Log($"#MainSceneLoader#----------Setting as active scene: {scene.name}");
                    SceneManager.SetActiveScene(scene);
                }
                
                //Debug.Log($"#MainSceneLoader#----------Scene successfully loaded: {scene.name}");
                
                _sceneHandles.Add(handle);
                _sceneLoadCount ++ ;
            }
        }
    }
}
