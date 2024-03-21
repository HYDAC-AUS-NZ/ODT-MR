using System.Collections;
using System.Collections.Generic;
using LightShaft.Scripts;
using UnityEngine;
using UnityEngine.Video;

public class YoutubeSimplified : MonoBehaviour
{
    private YoutubePlayer player;

    public string url;
    public bool autoPlay = true;
    public bool fullscreen = true;
    private VideoPlayer videoPlayer;

    private void Awake()
    {
        videoPlayer = GetComponentInChildren<VideoPlayer>();
        player = GetComponentInChildren<YoutubePlayer>();
        player.videoPlayer = videoPlayer;
    }

    private void Start()
    {
        player.dontForceFullscreen = true;
        Play();
    }

    public void Play()
    {
        if (fullscreen)
        {
            videoPlayer.renderMode = VideoRenderMode.CameraNearPlane;
            videoPlayer.aspectRatio = VideoAspectRatio.FitInside;
            videoPlayer.targetCamera = player.mainCamera;
            
        }
        player.autoPlayOnStart = autoPlay;
        player.videoQuality = YoutubePlayer.YoutubeVideoQuality.STANDARD;

        if(autoPlay)
            player.Play(url);
    }
}
