using UnityEngine;
using UnityEngine.Serialization;

using HYDAC.INFO;
using UnityEngine.AddressableAssets;

namespace HYDAC.INFO
{
    [CreateAssetMenu(fileName = "CInfo", menuName = "InfoSocs/Catalogue")]
    public class SCatalogueInfo : ProductInfo
    {
        [Header("Product Assets")]
        
        [FormerlySerializedAs("assemblyFolderKey")]
        [SerializeField] private string productAssetsKey;
        public string ProductAssetsKey => productAssetsKey;

        [SerializeField] private AssetReference assetsInfo;
        public AssetReference AssetsInfo => assetsInfo;
        
        protected override void ChangeFileName()
        {
#if UNITY_EDITOR
            string newFileName = "CInfo_" + this.productID + "_" + this.iname;
            string assetPath = UnityEditor.AssetDatabase.GetAssetPath(this.GetInstanceID());
            UnityEditor.AssetDatabase.RenameAsset(assetPath, newFileName);
#endif
        }
    }
}
