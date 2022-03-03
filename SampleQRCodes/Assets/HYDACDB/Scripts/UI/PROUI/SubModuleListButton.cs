using UnityEngine;
using UnityEngine.UI;

using TMPro;

using HYDACDB.INFO;
using HYDACDB.PRO;

public class SubModuleListButton : MonoBehaviour
{
    [SerializeField] private SocProductCallbacks productCallbacks;
    [SerializeField] private TextMeshProUGUI subModuleTitleTxt;
    [SerializeField] private Button button;

    private SSubModuleInfo _info; 

    public void Intitialise(SSubModuleInfo info)
    {
        _info = info;

        // Set name
        subModuleTitleTxt.text = string.Format("{0} - {1}", _info.ID.ToString("D2"), _info.iname);

        button.onClick.AddListener(OnButtonClick);
    }

    private void OnButtonClick()
    {
        //productNetWrapper.InvokeUISubModuleSelect(_info);
        productCallbacks.OnSubModuleSelected(_info);
    }
}
