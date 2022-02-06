using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.ResourceLocations;

namespace HYDACDB.ADD
{
    public static class AddressableLoader
    {
        internal static async Task LoadFromLabel(string label, IList<IResourceLocation> loadedLocations)
        {
            var unloadedLocations = await Addressables.LoadResourceLocationsAsync(label).Task;

            foreach (var location in unloadedLocations)
            {
                loadedLocations.Add(location);
            }
        }


        internal static async Task<IList<IResourceLocation>> LoadLabels(string[] labels)
        {
            IList<IResourceLocation> loadedLocations = new List<IResourceLocation>();// = new IList<IResourceLocation>();

            foreach (var label in labels)
            {
                IList<IResourceLocation> locationsOfLabel = new List<IResourceLocation>();

                await LoadFromLabel(label, locationsOfLabel);

                Debug.Log("#AddressableLocationLoader#-------Loaded assets with label: " + label);

                foreach (var location in locationsOfLabel)
                    loadedLocations.Add(location);
            }

            return loadedLocations;
        }



        internal static async Task<GameObject> LoadFromReference(AssetReference assetRef)
        {
            var loadedGameObject = await assetRef.LoadAssetAsync<GameObject>().Task;

            return loadedGameObject;
        }


        internal static async Task<IList<GameObject>>LoadAssetReferences(AssetReference[] assetrefs)
        {
            IList<GameObject> loadedGameObjects = new List<GameObject>();

            foreach (var assetRef in assetrefs)
            {
                IList<GameObject> gameObjectsOfReferences = new List<GameObject>();

                loadedGameObjects.Add(await LoadFromReference(assetRef));

                // Debug.Log("#AddressableLocationLoader#-------Loaded asset: " + assetRef.AssetGUID);
            }

            return loadedGameObjects;
        }


        internal static async Task<GameObject> InstantiateFromReference(AssetReference assetRef, Transform parent)
        {
            var temp = await Addressables.InstantiateAsync(assetRef, parent).Task;

            return temp;
        }


        internal static void ReleaseObject(GameObject assetObject)
        {
            Addressables.ReleaseInstance(assetObject);
        }
    }
}
