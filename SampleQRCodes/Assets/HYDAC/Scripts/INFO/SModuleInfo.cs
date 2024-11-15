using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Video;

namespace HYDAC.INFO
{
    [CreateAssetMenu(fileName = "ModuleInfo", menuName = "InfoSocs/Module")]
    public class SModuleInfo : ProductInfo
    {
        public bool isStatic;
        
        [SerializeField] private AssetReference highPolyReference;
        [SerializeField] private AssetReference infoUIReference = null;
        [SerializeField] private VideoClip videoClip = null;

        [SerializeField] private SSubModuleInfo[] subModules;

        public AssetReference HighPolyReference => highPolyReference;
        public AssetReference InfoUIReference => infoUIReference;
        public VideoClip VideoClip => videoClip;

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
