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

        protected override void OnUIComponentOpened(SAssetsInfo assetInfo)
        {
            base.OnUIComponentOpened(assetInfo);

            playbackButton.IsToggled = false;

            videoPlayer.Play((assetInfo.VideoURL.Equals("")) ? placeHolderVideoURL : assetInfo.VideoURL);
        }

        protected override void OnUIComponentClosed()
        {
            base.OnUIComponentClosed();

            videoPlayer.Stop();
        }
    }
}

