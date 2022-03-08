using UnityEngine;

namespace HYDAC.INFO
{
    public abstract class ProductInfo : ScriptableObject
    {
        public bool rename = false;
        
        [Space]
        [Header("Main Information")] 
        public string productID;
        public int ID;

        public string iname;
        [TextArea] public string description;
        
        protected abstract void ChangeFileName();

        private void OnValidate()
        {
            if (this.rename)
            {
                ChangeFileName();
                this.rename = false;
            }
        }
    }
}
