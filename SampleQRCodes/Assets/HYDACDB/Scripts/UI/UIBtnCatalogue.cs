using UnityEngine;
using UnityEngine.UI;

using TMPro;

using HYDACDB.INFO;
using HYDACDB.PRO;

namespace HYDACDB.UI
{
    public class UIBtnCatalogue : MonoBehaviour
    {
        [SerializeField] private SocProductCallbacks productCallbacks;
        [SerializeField] private TextMeshProUGUI nameText;
        [SerializeField] private Image productImage;
        [SerializeField] private Button productButton;

        private SCatalogueInfo info;
        private Transform _parent;
        
        public void Initialize(SCatalogueInfo _info, Transform uiParent)
        {
            _parent = uiParent;
            
            info = _info;

            nameText.text = info.iname;
            productImage.sprite = _info.ProductImage;
            
            if(info.isLoadable)
                productButton.onClick.AddListener(OnButtonClicked);
        }

        private void OnButtonClicked()
        {
            Debug.Log("#UIBtnCatalogue#------------Catalogue button clicked: " + info.iname);

            productCallbacks.OnProductSelected(info);
            
            if(info.isLoadable)
                productButton.onClick.RemoveListener(OnButtonClicked);
            
            _parent.gameObject.SetActive(false);
            
            info = null;

            nameText.text = "";
            productImage.sprite = null;
        }
    }
}