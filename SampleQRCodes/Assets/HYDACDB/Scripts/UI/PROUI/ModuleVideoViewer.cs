using UnityEngine;

using LightShaft.Scripts;
using Microsoft.MixedReality.Toolkit.UI;

using HYDACDB.INFO;

namespace HYDACDB.UI
{
    public class ModuleVideoViewer : BaseProductUI
    {
        [Header("Video Viewer Members")]
        [SerializeField] private YoutubePlayer videoPlayer;
        [SerializeField] private YoutubeVideoController videocontroller;
        [SerializeField] private Interactable playbackButton;
        [SerializeField] private string placeHolderVideoURL;
        private void Start()
        {
            //playbackButton.IsInteractive = PlayerPrefs.GetInt(NetPlayerProperties.PLAYERPROPS_ROLE) == 0;
        }

        protected override void OnEnable()
        {
            base.OnEnable();

            productCallbacks.EVideoPlay += OnVideoTogglePlayback;
        }

        protected override void OnDisable()
        {
            base.OnDisable();

            productCallbacks.EVideoPlay -= OnVideoTogglePlayback;
        }

        private void OnVideoTogglePlayback()
        {
            videocontroller.PlayToggle();
        }

        protected override void OnModuleChanged(SModuleInfo newModule)
        {
            videoPlayer.gameObject.SetActive(true);
            
            base.OnModuleChanged(newModule);

            StopAllCoroutines();
            
            videoPlayer.Stop();
            playbackButton.IsToggled = false;

            //playbackButton.IsInteractive = PlayerPrefs.GetInt(NetPlayerProperties.PLAYERPROPS_ROLE) == 0;
            
            videoPlayer.PreLoadVideo((newModule.VideoURL.Equals("")) ? placeHolderVideoURL :  newModule.VideoURL);
            
            videocontroller.Play();
            videocontroller.Pause();
        }
    }
}