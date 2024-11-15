using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Video;

namespace HYDAC.INFO
{
    [CreateAssetMenu(fileName = "AInfo", menuName = "InfoSocs/AssetsInfo")]
    public class SAssetsInfo : ScriptableObject
    {
        [Space]
        public bool hasSchematic;
        public bool hasModel;
        public bool isModelStatic = false;
        public bool hasDocumentation;
        public bool hasVideo;

        [Space]
        [SerializeField] private AssetReference schematicReference;
        [SerializeField] private AssetReference modelReference;
        [SerializeField] private AssetReference documentationReference = null;
        [SerializeField] private VideoClip videoClip = null;

        public AssetReference SchematicReference => schematicReference;
        public AssetReference HighPolyReference => modelReference;
        public AssetReference InfoUIReference => documentationReference;
        public VideoClip VideoClip => videoClip;
    }
}
