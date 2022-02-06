using UnityEngine;
using UnityEngine.AddressableAssets;

namespace HYDACDB.INFO
{
    [CreateAssetMenu(fileName = "ModuleInfo", menuName = "AssemblyInfos/Module")]
    public class SModuleInfo : ProductInfo
    {
        public bool isStatic;
        
        [SerializeField] private AssetReference highPolyReference;
        [SerializeField] private AssetReference infoUIReference = null;
        [SerializeField] private string videoURL = null;

        [SerializeField] private SSubModuleInfo[] subModules;

        public AssetReference HighPolyReference => highPolyReference;
        public AssetReference InfoUIReference => infoUIReference;
        public string VideoURL => videoURL;

        public SSubModuleInfo[] SubModules => subModules;
        public void SetSubModules(SSubModuleInfo[] subModuleInfos) { subModules = subModuleInfos; }


        protected override void ChangeFileName()
        {
#if UNITY_EDITOR
            string newFileName = "MInfo_" + ID + "_" + iname;
            string assetPath = UnityEditor.AssetDatabase.GetAssetPath(this.GetInstanceID());
            UnityEditor.AssetDatabase.RenameAsset(assetPath, newFileName);
#endif
        }
    }
}
