using UnityEngine;

namespace HYDACDB.INFO
{
    /// <summary>
    /// <c>SocMachinePartInfo</c> is a scriptable object class that contains all the main details
    /// of a given machine part such as:
    /// <c>partName</c><value>This is the name of the part in, partName.</value>
    /// </summary>
    public class SSubModuleInfo : ProductInfo
    {
        public void PrintInfo()
        {
            Debug.LogFormat("#SSubModule#-------------------------{0}{1}\nPartInfo: {2}", 
                ID, iname , description);
        }

        protected override void ChangeFileName()
        {
#if UNITY_EDITOR
            string newFileName = "SInfo_" + ID + "_" + iname;
            string assetPath = UnityEditor.AssetDatabase.GetAssetPath(this.GetInstanceID());
            UnityEditor.AssetDatabase.RenameAsset(assetPath, newFileName);
#endif
        }
    }
}