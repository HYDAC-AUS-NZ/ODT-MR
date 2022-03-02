using UnityEngine;
using UnityEngine.Serialization;

using HYDACDB.INFO;

namespace HYDAC.INFO
{
    [CreateAssetMenu(fileName = "CInfo", menuName = "InfoSocs/Catalogue")]
    public class SCatalogueInfo : ProductInfo
    {
        [Header("Product Assets")]
        
        [FormerlySerializedAs("assemblyFolderKey")]
        [SerializeField] private string productAssetsKey;
        public string ProductAssetsKey => productAssetsKey;

        [SerializeField] private SAssetsInfo assetsInfo;
        public SAssetsInfo AssetsInfo => assetsInfo;
        
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
