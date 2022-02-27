/*
MIT License
Copyright (c) 2021 Anton Kesy
Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:
The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.
THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
 */

using UnityEngine;

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