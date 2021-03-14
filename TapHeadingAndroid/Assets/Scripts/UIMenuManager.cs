using System;
using System.Collections;
using System.Collections.Generic;
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

    private bool _isSoundOn;
    [SerializeField] private UIFader soundOnFader;
    [SerializeField] private UIFader soundOffFader;

    private void Start()
    {
        _titlePosition = gameTitleTransform.position;
    }

    internal void FadeInMenu()
    {
        foreach (var fader in faders)
        {
            fader.Fade(true, fadeInOutDuration);
        }

        if (_isSoundOn)
        {
            soundOnFader.Fade(true, fadeInOutDuration);
            soundOffFader.gameObject.SetActive(false);
        }
        else
        {
            soundOffFader.Fade(true, fadeInOutDuration);
            soundOnFader.gameObject.SetActive(false);
        }
    }

    internal void FadeOutMenu()
    {
        gameTitleFader.Fade(false, fadeInOutDuration);
        foreach (var fader in faders)
        {
            fader.Fade(false, fadeInOutDuration * 2);
        }

        Debug.Log("FadeOutMenu");
        soundOffFader.Fade(false, 0);
        soundOnFader.Fade(false, 0);

        /*
        if (_isSoundOn)
        {
            soundOnFader.gameObject.SetActive(false);
            soundOffFader.Fade(false, fadeInOutDuration * 2);
        }
        else
        {
            soundOffFader.gameObject.SetActive(false);
            soundOnFader.Fade(false, fadeInOutDuration * 2);
        }*/
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


        yield return new WaitForSecondsRealtime(titleMenuDelay);
        foreach (var fader in faders)
        {
            fader.Fade(true, fadeInOutDuration);
        }

        if (_isSoundOn)
        {
            soundOnFader.Fade(true, fadeInOutDuration);
            soundOffFader.gameObject.SetActive(false);
        }
        else
        {
            soundOffFader.Fade(true, fadeInOutDuration);
            soundOnFader.gameObject.SetActive(false);
        }

        yield return null;
    }

    internal void SlideInGameTitle()
    {
        gameTitleTransform.position = titleStartTransform.position;
        StartCoroutine(SlideIn(gameTitleTransform, _titlePosition, titleLerpDuration));
    }

    private IEnumerator SlideIn(Transform transformSlideObject, Vector3 toPosition, float duration)
    {
        var counter = 0f;

        while (counter < duration)
        {
            counter += Time.deltaTime;
            var position = gameTitleTransform.position;
            position = Vector3.Lerp(position, toPosition, counter / duration);
            gameTitleTransform.position = position;

            yield return null;
        }
    }

    internal void SetSound(bool isSoundOn)
    {
        _isSoundOn = isSoundOn;
    }
}