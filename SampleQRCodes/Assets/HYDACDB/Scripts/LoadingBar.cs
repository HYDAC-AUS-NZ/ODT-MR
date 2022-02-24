using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace HYDACDB
{
    public class LoadingBar : MonoBehaviour
    {
        public static LoadingBar Instance;

        [SerializeField] private Canvas loadingUI;
        [SerializeField] private Slider loadingBar;
        [SerializeField] private TextMeshProUGUI loadingText;

        private void Awake()
        {
            if (Instance)
                Destroy(Instance);
            Instance = this;
            
            loadingUI.enabled = false;
        
            loadingBar.minValue = 0;
            loadingBar.maxValue = 100;
            loadingBar.value = 0;
        }

        public void StartLoading(string text)
        {
            loadingUI.enabled = true;
            loadingText.text = text;
            
            loadingBar.value = 0;
        }

        
        public void StopLoading()
        {
            loadingUI.enabled = false;
        }
        
        
        public void SetSliderValue(float value)
        {
            loadingBar.value = value;
        }
    }
}
