using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Serialization;

using TMPro;

using HYDACDB.INFO;
//using HYDAC.PRO;

namespace HYDACDB.UI
{
    public class UIBtnCatalogue : MonoBehaviour
    {
        //[FormerlySerializedAs("assemblyUI")] [SerializeField] private SocProductNetWrapper productNetWrapper;
        [SerializeField] private TextMeshProUGUI nameText;
        [SerializeField] private Image productImage;
        [SerializeField] private Button productButton;

        private SCatalogueInfo info;
        
        public void Initialize(SCatalogueInfo _info)
        {
            info = _info;

            nameText.text = info.iname;
            productImage.sprite = _info.ProductImage;

            if (!info.isLoadable)
            {
                nameText.color = Color.grey;
                productImage.color = Color.grey;
                return;
            }
            
            productButton.onClick.AddListener(OnButtonClicked);
        }

        private void OnButtonClicked()
        {
            Debug.Log("#UIBtnCatalogue#------------Catalogue button clicked: " + info.iname);

            //productNetWrapper.InvokeUIProductSelect(info);
            
            if(info.isLoadable)
                productButton.onClick.RemoveListener(OnButtonClicked);
            
            info = null;

            nameText.text = "";
            productImage.sprite = null;
        }
    }
}