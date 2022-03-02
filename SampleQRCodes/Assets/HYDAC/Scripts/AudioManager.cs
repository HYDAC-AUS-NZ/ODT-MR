using UnityEngine;

namespace HYDAC.Audio
{
    public class AudioManager : MonoBehaviour
    {
        public static AudioManager Instance;

        #region variables
        [Header("Audio Source")]
        public AudioSource audioSource;

        [Header("Audio Clips")]
        public AudioClip buttonClick;

        public AudioClip qrScanned;

        #endregion

        #region private functions
        /// <summary>
        /// make sure we dont have an instance if we do destroy it
        /// </summary>
        private void Awake()
        {
            if (Instance)
                Destroy(Instance);
            Instance = this;
        }
        #endregion

        #region public functions
        /// <summary>
        /// plays the audio clip
        /// </summary>
        /// <param name="clip"></param>
        public void PlayClip(AudioClip clip)
        {
            audioSource.PlayOneShot(clip);
        }
        #endregion
    }
}