using System.Collections;

using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

using HYDAC.INFO;
using HYDAC.UI;

public class UISchematicViewer : UIComponents
{
    [Header("Schematic Viewer Members")]
    [SerializeField] private MeshRenderer schematicRenderer;

    private AsyncOperationHandle<Material> _materialLoadHandle;
    private Material _currentSchematicMaterial;

    // Start is called before the first frame update
    protected override void OnUIComponentOpened(SAssetsInfo assetInfo)
    {
        base.OnUIComponentOpened(assetInfo);
        
        // Load Submodule Model
        StartCoroutine(LoadSchematic(assetInfo));
    }

    private IEnumerator LoadSchematic(SAssetsInfo assetInfo)
    {
        if(_currentSchematicMaterial != null)
            Addressables.Release(_currentSchematicMaterial);
        
        _materialLoadHandle = Addressables.LoadAssetAsync<Material>(assetInfo.SchematicReference);
        yield return new WaitUntil(() => _materialLoadHandle.IsDone);

        _currentSchematicMaterial = _materialLoadHandle.Result;
        
        schematicRenderer.materials = new Material[] {_currentSchematicMaterial};
    }
}
