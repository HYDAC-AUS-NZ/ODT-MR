using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

namespace HYDAC.UI
{
    public class HYDACVideoPlayerController : MonoBehaviour
    {
        public VideoPlayer videoPlayer;  // The VideoPlayer component
        public Slider scrubber;          // Slider for the scrubber
        public TextMeshPro playPauseButtonText; // Text component of play/pause button (optional)

        [Header("Optionals")]
        public VideoClip startingVideoClip;
        public AudioSource volumeSource;


        private bool isDragging = false;
        private const double skipTime = 15.0; // Time in seconds to skip

        void Start()
        {
            if (videoPlayer == null || scrubber == null)
            {
                Debug.LogError("Please assign all UI components in the inspector.");
                return;
            }

            videoPlayer.loopPointReached += OnVideoEnd;

            // Initialize UI elements
            scrubber.minValue = 0;
            scrubber.maxValue = 1;
            
            scrubber.onValueChanged.AddListener(OnScrubberChanged);
            UpdatePlayPauseButtonText();

            if(startingVideoClip != null)
            {
                PlayVideoClip(startingVideoClip);
            }

            OnVolumeChanged(1);
        }

        void Update()
        {
            if (!isDragging)
            {
                // Update the scrubber based on video playback
                scrubber.value = (float)(videoPlayer.time / videoPlayer.length);
            }
        }

        public void TogglePlayPause()
        {
            if (videoPlayer.isPlaying)
            {
                videoPlayer.Pause();
            }
            else
            {
                videoPlayer.Play();
            }
            UpdatePlayPauseButtonText();
        }

        private void UpdatePlayPauseButtonText()
        {
            if (playPauseButtonText != null)
            {
                playPauseButtonText.text = videoPlayer.isPlaying ? "Pause" : "Play";
            }
        }

        private void OnScrubberChanged(float value)
        {
            if (isDragging)
            {
                videoPlayer.time = value * videoPlayer.length;
            }
        }

        public void OnBeginDrag()
        {
            isDragging = true;
        }

        public void OnEndDrag()
        {
            isDragging = false;
            videoPlayer.time = scrubber.value * videoPlayer.length;
        }

        public void OnVolumeChanged(float value)
        {
            volumeSource.volume = value;
        }

        private void OnVideoEnd(VideoPlayer vp)
        {
            UpdatePlayPauseButtonText();
        }

        // New Method to Play a Specified Video Clip
        public void PlayVideoClip(VideoClip clip)
        {
            videoPlayer.clip = clip;
            videoPlayer.Play();
            UpdatePlayPauseButtonText();
        }

        // New Method to Stop the Video
        public void StopVideo()
        {
            videoPlayer.Stop();
            scrubber.value = 0;
            UpdatePlayPauseButtonText();
        }

        // New Method to Skip Forward 15 Seconds
        public void SkipForward()
        {
            videoPlayer.time = Mathf.Min((float)(videoPlayer.time + skipTime), (float)videoPlayer.length);
        }

        // New Method to Skip Backward 15 Seconds
        public void SkipBackward()
        {
            videoPlayer.time = Mathf.Max((float)(videoPlayer.time - skipTime), 0f);
        }
    }
}