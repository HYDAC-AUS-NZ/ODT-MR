using UnityEngine;

using Microsoft.MixedReality.Toolkit.UI;

using HYDAC.INFO;

namespace HYDAC.UI
{
    public class UIVideoPlayer : UIComponents
    {
        [Header("Video Viewer Members")]
        [SerializeField] private HYDACVideoPlayerController videoPlayerController;
        [SerializeField] private Interactable playbackButton;

        protected override void OnUIComponentOpened(SAssetsInfo assetInfo)
        {
            base.OnUIComponentOpened(assetInfo);

            playbackButton.IsToggled = false;

            videoPlayerController.PlayVideoClip(assetInfo.VideoClip);
        }

        protected override void OnUIComponentClosed()
        {
            base.OnUIComponentClosed();

            videoPlayerController.StopVideo();
        }
    }
}

