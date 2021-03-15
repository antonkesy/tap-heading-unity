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

using System.Collections;
using UnityEngine;

public class UIMenuManager : MonoBehaviour
{
    [SerializeField] private UIFader[] faders;
    [SerializeField] private UIFader gameTitleFader;
    [SerializeField] private Transform gameTitleTransform;
    private Vector3 _titlePosition;
    [SerializeField] private Transform titleStartTransform;
    [SerializeField] private float titleLerpDuration;
    [SerializeField] private float titleMenuDelay;

    [SerializeField] private float fadeInOutDuration = .5f;

    private bool _isSoundOff;
    [SerializeField] private UIFader soundOnFader;
    [SerializeField] private UIFader soundOffFader;

    [SerializeField] private UIFader newHighScoreFader;

    private void Start()
    {
        _titlePosition = gameTitleTransform.position;
    }

    internal void FadeInMenu()
    {
        soundOffFader.Fade(false, 0);
        soundOnFader.Fade(false, 0);
        foreach (var fader in faders)
        {
            fader.Fade(true, fadeInOutDuration);
        }

        if (_isSoundOff)
        {
            soundOffFader.Fade(false, 0f);
            soundOnFader.Fade(true, fadeInOutDuration);
        }
        else
        {
            soundOnFader.Fade(false, 0f);
            soundOffFader.Fade(true, fadeInOutDuration);
        }
    }

    internal void FadeOutMenu()
    {
        gameTitleFader.Fade(false, fadeInOutDuration);
        foreach (var fader in faders)
        {
            fader.Fade(false, fadeInOutDuration * 3);
        }

        newHighScoreFader.Fade(false, fadeInOutDuration * 3);

        if (_isSoundOff)
        {
            soundOffFader.Fade(false, 0f);
            soundOnFader.Fade(false, fadeInOutDuration * 3);
        }
        else
        {
            soundOnFader.Fade(false, 0f);
            soundOffFader.Fade(false, fadeInOutDuration * 3);
        }
    }

    internal void FadeInStart()
    {
        StartCoroutine(WaitForGameTitle());
    }

    private IEnumerator WaitForGameTitle()
    {
        foreach (var fader in faders)
        {
            fader.Fade(false, 0);
        }

        soundOffFader.Fade(false, 0);
        soundOnFader.Fade(false, 0);
        newHighScoreFader.Fade(false, 0);


        yield return new WaitForSecondsRealtime(titleMenuDelay);
        foreach (var fader in faders)
        {
            fader.Fade(true, fadeInOutDuration);
        }

        if (_isSoundOff)
        {
            soundOffFader.Fade(false, 0f);
            soundOnFader.Fade(true, fadeInOutDuration);
        }
        else
        {
            soundOnFader.Fade(false, 0f);
            soundOffFader.Fade(true, fadeInOutDuration);
        }

        yield return null;
    }

    internal void SlideInGameTitle()
    {
        gameTitleTransform.position = titleStartTransform.position;
        gameTitleFader.Fade(true, 0);
        StartCoroutine(SlideIn(gameTitleTransform, _titlePosition, titleLerpDuration));
    }

    private IEnumerator SlideIn(Transform transformSlideObject, Vector3 toPosition, float duration)
    {
        var counter = 0f;

        while (counter < duration)
        {
            counter += Time.deltaTime;
            var position = transformSlideObject.position;
            position = Vector3.Lerp(position, toPosition, counter / duration);
            transformSlideObject.position = position;

            yield return null;
        }
    }

    internal void SetSound(bool isSoundOn)
    {
        _isSoundOff = isSoundOn;
        if (!_isSoundOff)
        {
            soundOffFader.Fade(true, 0f);
            soundOnFader.Fade(false, 0f);
        }
        else
        {
            soundOnFader.Fade(true, 0f);
            soundOffFader.Fade(false, 0f);
        }
    }

    internal void FadeInNewHighScore()
    {
        newHighScoreFader.Fade(true, fadeInOutDuration);
    }
}