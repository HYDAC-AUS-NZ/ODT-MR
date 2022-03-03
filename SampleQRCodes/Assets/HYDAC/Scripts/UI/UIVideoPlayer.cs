using UnityEngine;

using Microsoft.MixedReality.Toolkit.UI;
using LightShaft.Scripts;

using HYDAC.INFO;


namespace HYDAC.UI
{
    public class UIVideoPlayer : UIComponents
    {
        [Header("Video Viewer Members")]
        [SerializeField] private YoutubePlayer videoPlayer;
        [SerializeField] private Interactable playbackButton;
        [SerializeField] private string placeHolderVideoURL;

        protected override void OnEnable()
        {
            base.OnEnable();

            qrCallbacks.EOnQRVideoToggle += OnVideoPlayerToggled;
        }

        protected override void OnDisable()
        {
            base.OnDisable();

            qrCallbacks.EOnQRVideoToggle -= OnVideoPlayerToggled;
        }

        private void OnVideoPlayerToggled(bool toggle, SAssetsInfo assetsInfo)
        {
            if (toggle)
            {
                UIObject.gameObject.SetActive(true);

                playbackButton.IsToggled = false;

                videoPlayer.Play((assetsInfo.VideoURL.Equals("")) ? placeHolderVideoURL : assetsInfo.VideoURL);
            }
            else
            {
                videoPlayer.Stop();
                UIObject.gameObject.SetActive(false);
            }

            //var relativePath = "https://drive.google.com/file/d/1gS5Br7IeAojYiydF1v7wYHJuG06IWz3_/view?usp=sharing";

            //UnityEngine.WSA.Launcher.LaunchFile(UnityEngine.WSA.Folder.DocumentsLibrary, relativePath, false);
        }
    }
}

