using UnityEngine;
using UnityEngine.Serialization;

public class AudioManager : MonoBehaviour
{
    private AudioSource _audioSource;
    [SerializeField] private AudioClip collectCoinAudioClip;
    [SerializeField] private float coinVolume;
    [SerializeField] private AudioClip destroyPlayerAudioClip;
    [SerializeField] private float destroyVolume;
    [SerializeField] private AudioClip tapPlayerAudioClip;
    [SerializeField] private float tapVolume;
    [SerializeField] private AudioClip tapUIAudioClip;
    [SerializeField] private float uiVolume;
    [SerializeField] private AudioClip newHighSoreAudioClip;
    [SerializeField] private float newHighScoreVolume;
    [SerializeField] private AudioClip startApplicationAudioClip;
    [SerializeField] private float startApplicationVolume;


    private bool _isSoundOn;

    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    internal void SetSound(bool isSoundOn)
    {
        _isSoundOn = isSoundOn;
    }

    private void PlayClip(AudioClip clip, float volume)
    {
        if (!_isSoundOn) return;
        _audioSource.PlayOneShot(clip, volume);
    }

    internal void PlayCollectCoin()
    {
        PlayClip(collectCoinAudioClip, coinVolume);
    }

    internal void PlayDestroyPlayer()
    {
        PlayClip(destroyPlayerAudioClip, destroyVolume);
    }

    internal void PlayTapPlayer()
    {
        PlayClip(tapPlayerAudioClip, tapVolume);
    }

    internal void PlayTapUI()
    {
        PlayClip(tapUIAudioClip, uiVolume);
    }

    internal void PlayNewHighScore()
    {
        PlayClip(newHighSoreAudioClip, newHighScoreVolume);
    }

    internal void PlayStartApplication()
    {
        PlayClip(startApplicationAudioClip, startApplicationVolume);
    }
}