using System.Collections.Generic;
using UnityEngine;

namespace HYDACDB.PRO
{
    public class ProductSubModule : AProduct 
    {
        [SerializeField]
        private MeshRenderer[] meshes;

        private void Awake()
        {
            GetAllMeshes();
        }

        private void  GetAllMeshes()
        {
            List<MeshRenderer> meshList = new List<MeshRenderer>();

            if(TryGetComponent<MeshRenderer>(out MeshRenderer mesh))
            {
                meshList.Add(mesh);
            }

            if(transform.childCount > 0)
            {
                var childMeshes = GetComponentsInChildren<MeshRenderer>();

                foreach(var childMesh in childMeshes)

                    meshList.Add(childMesh);
            }

            meshes = meshList.ToArray();
        }

        internal void ToggleMeshes(bool toggle)
        {
            foreach(var mesh in meshes)
            {
                mesh.enabled = toggle;
            }
        }
    }
}
