using UnityEngine;

namespace tap_heading.Audio
{
    public class AudioManager : MonoBehaviour, IAudioManager
    {
        private AudioSource _audioSource;

        [Header("Collect Coin")] [SerializeField]
        private AudioClip collectCoinAudioClip;

        [SerializeField] private float coinVolume;

        [Header("Destroy Player")] [SerializeField]
        private AudioClip destroyPlayerAudioClip;

        [SerializeField] private float destroyVolume;

        [Header("Tap Player")] [SerializeField]
        private AudioClip tapPlayerAudioClip;

        [SerializeField] private float tapVolume;
        [Header("Tap UI")] [SerializeField] private AudioClip tapUIAudioClip;
        [SerializeField] private float uiVolume;

        [Header("New HighScore")] [SerializeField]
        private AudioClip newHighSoreAudioClip;

        [SerializeField] private float newHighScoreVolume;

        [Header("Start Application")] [SerializeField]
        private AudioClip startApplicationAudioClip;

        [SerializeField] private float startApplicationVolume;


        private bool _isSoundOn;

        private void Start()
        {
            _audioSource = GetComponent<AudioSource>();
        }

        private void PlayClip(AudioClip clip, float volume)
        {
            if (!_isSoundOn) return;
            _audioSource.PlayOneShot(clip, volume);
        }

        public void SetSound(bool isActive)
        {
            _isSoundOn = isActive;
        }

        public void PlayStartApplication()
        {
            PlayClip(startApplicationAudioClip, startApplicationVolume);
        }

        public void PlayPlayerDeath()
        {
            PlayClip(destroyPlayerAudioClip, destroyVolume);
        }

        public void PlayPlayerTap()
        {
            PlayClip(tapPlayerAudioClip, tapVolume);
        }

        public void PlayUITap()
        {
            PlayClip(tapUIAudioClip, uiVolume);
        }

        public void PlayNewHighScore()
        {
            PlayClip(newHighSoreAudioClip, newHighScoreVolume);
        }

        public void PlayCollectCoin()
        {
            PlayClip(collectCoinAudioClip, coinVolume);
        }
    }
}