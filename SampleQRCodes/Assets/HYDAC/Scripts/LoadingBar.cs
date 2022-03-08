using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace HYDAC
{
    public class LoadingBar : MonoBehaviour
    {
        public static LoadingBar Instance;

        [SerializeField] private GameObject loadingUI;
        [SerializeField] private Slider loadingBar;
        [SerializeField] private TextMeshProUGUI loadingText;

        private void Awake()
        {
            if (Instance)
                Destroy(Instance);
            Instance = this;
            
            loadingUI.SetActive(false);
        
            loadingBar.minValue = 0;
            loadingBar.maxValue = 100;
            loadingBar.value = 0;
        }

        public void StartLoading(string text)
        {
            loadingUI.SetActive(true);
            loadingText.text = text;
            
            loadingBar.value = 0;
        }

        
        public void StopLoading()
        {
            loadingUI.SetActive(false);
        }
        
        
        public void SetSliderValue(float value)
        {
            loadingBar.value = value;
        }
    }
}
