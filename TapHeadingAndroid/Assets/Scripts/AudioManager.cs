using UnityEngine;

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

    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    private void PlayClip(AudioClip clip, float volume)
    {
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
}