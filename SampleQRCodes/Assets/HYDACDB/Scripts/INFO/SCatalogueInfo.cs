using System.Collections.Generic;

using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Serialization;

namespace HYDACDB.INFO
{
    
    public class SCatalogueInfo : ProductInfo
    {
        public bool isLoadable;

        public List<SModuleInfo> Modules = new List<SModuleInfo>();       
        
        [Header("Product Assets")]
        
        [FormerlySerializedAs("assemblyFolderKey")]
        [SerializeField] private string productAssetsKey;
        public string ProductAssetsKey => productAssetsKey;
        
        
        [SerializeField] private Sprite productImage;
        public Sprite ProductImage => productImage;

        
        [SerializeField] private AssetReference schematicReference;
        public AssetReference SchematicReference => schematicReference;
        
        
        [FormerlySerializedAs("assemblyPrefab")] [SerializeField] private AssetReference productPrefab;
        public AssetReference ProductPrefab => productPrefab;
        
        protected override void ChangeFileName()
        {
#if UNITY_EDITOR
            string newFileName = "CInfo_" + ID + "_" + iname;
            string assetPath = UnityEditor.AssetDatabase.GetAssetPath(this.GetInstanceID());
            UnityEditor.AssetDatabase.RenameAsset(assetPath, newFileName);
#endif
        }
    }
}
