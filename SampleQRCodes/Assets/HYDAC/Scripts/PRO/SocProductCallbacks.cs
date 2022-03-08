using System;
using HYDAC.INFO;
using UnityEngine;

namespace HYDAC.PRO
{
    [CreateAssetMenu(menuName = "Socks/Assembly/Events", fileName = "SOC_AssemblyEvents")]
    public class SocProductCallbacks : ScriptableObject
    {
        [Range(0.1f, 5.0f)]
        [SerializeField] private float explodeTime;
        public float ExplodeTime => explodeTime;

        [Space]
        public SModuleInfo[] Modules;

        // Current selected module
        public SModuleInfo CurrentModule { get; private set; }
        // Current selected subModule
        public SSubModuleInfo CurrentSubModule { get; private set; }


        private void Awake()
        {
            CurrentModule = null;
        }


        // On Assembly selected event and method
        public event Action<SCatalogueInfo> EAssemblySelected;
        internal void OnProductSelected(SCatalogueInfo info)
        {
            EAssemblySelected?.Invoke(info);
        }


        // On module selected
        public event Action<SModuleInfo> EModuleSelected;
        internal void OnModuleSelected(SModuleInfo info)
        {
            //Debug.Log("#SocAssemblyEvents#------------OnChangeModule: " + info.iname);

            CurrentModule = info;

            EModuleSelected?.Invoke(info);
        }


        // On module explode
        public event Action<bool> EModuleExplode;
        public void OnModuleExplode(bool toggle)
        {
            Debug.Log("#SocAssemblyEvents#------------OnModuleExplode: " + toggle);

            EModuleExplode?.Invoke(toggle);
        }


        // On Sub-Module selected
        public event Action<SSubModuleInfo> ESubModuleSelected;
        internal void OnSubModuleSelected(SSubModuleInfo info)
        {
            Debug.Log("#SocAssemblyEvents#------------OnSelectSubModule: " + info.iname);

            CurrentSubModule = info;

            ESubModuleSelected?.Invoke(info);
        }
    }
}