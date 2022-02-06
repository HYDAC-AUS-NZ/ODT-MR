using System.Threading.Tasks;

using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.ResourceProviders;

namespace HYDACDB.ADD
{
    static class AddressablesSceneLoader
    {
        internal static async Task<SceneInstance> LoadScene(AssetReference sceneReference, bool isAdditive)
        {
            LoadSceneMode loadSceneMode = (isAdditive)? LoadSceneMode.Additive : LoadSceneMode.Single;

            SceneInstance scene = await Addressables.LoadSceneAsync(sceneReference, LoadSceneMode.Additive).Task;

            Debug.Log("#AddressablesSceneLoader#-------------Scene Loaded: " + scene.Scene.name);

            return scene;
        }

        internal static async Task UnloadScene(SceneInstance sceneToUnload)
        {
            Debug.Log("#AddressablesSceneLoader#-------------Scene Unloaded: " + sceneToUnload.Scene.name);

            await Addressables.UnloadSceneAsync(sceneToUnload).Task;
        }
    }
}
