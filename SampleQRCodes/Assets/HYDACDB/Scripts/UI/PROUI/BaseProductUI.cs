using UnityEngine;

using HYDACDB.INFO;
using HYDACDB.PRO;
using UnityEngine.Serialization;

namespace HYDACDB.UI
{
    public class BaseProductUI : MonoBehaviour
    {
        [FormerlySerializedAs("assemblyEvents")] [SerializeField] protected SocProductCallbacks productCallbacks;

        protected SModuleInfo _currentModuleInfo;

        protected virtual void OnEnable()
        {
            productCallbacks.EModuleSelected += OnModuleChanged;
        }
        protected virtual void OnDisable()
        {
            productCallbacks.EModuleSelected -= OnModuleChanged;
        }

        protected virtual void OnModuleChanged(SModuleInfo newModule) 
        {
            _currentModuleInfo = newModule;
        }
    }
}
