using UnityEngine;
using UnityEngine.AddressableAssets;

namespace HYDAC.INFO
{
    [CreateAssetMenu(fileName = "AssetsInfo", menuName = "InfoSocs/AssetsInfo")]
    public class SAssetsInfo : ScriptableObject
    {
        [SerializeField] private string productAssetsKey;

        [Space]
        public bool hasSchematic;
        public bool hasModel;
        public bool hasDocumentation;
        public bool hasVideo;

        [Space]
        [SerializeField] private AssetReference schematicReference;
        [SerializeField] private AssetReference highPolyReference;
        [SerializeField] private AssetReference infoUIReference = null;
        [SerializeField] private string videoURL = null;

        public string ProductAssetsKey => productAssetsKey;
        public AssetReference SchematicReference => schematicReference;
        public AssetReference HighPolyReference => highPolyReference;
        public AssetReference InfoUIReference => infoUIReference;
        public string VideoURL => videoURL;
    }
}
