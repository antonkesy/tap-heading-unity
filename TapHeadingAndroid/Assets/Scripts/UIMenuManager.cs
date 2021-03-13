using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIMenuManager : MonoBehaviour
{
    [SerializeField] private UIFader[] faders;
    [SerializeField] private Transform gameTitleTransform;
    [SerializeField] private float titlePosition;
    [SerializeField] private Transform titleStartTransform;

    private void Start()
    {
        titlePosition = gameTitleTransform.position.y;
    }

    internal void FadeInMenu()
    {
        foreach (var fader in faders)
        {
            fader.Fade(true);
        }
    }

    internal void FadeOutMenu()
    {
        foreach (var fader in faders)
        {
            fader.Fade(false);
        }
    }

    internal void SlideInGameTitle()
    {
        gameTitleTransform.position = titleStartTransform.position;
        StartCoroutine(SlideIn(gameTitleTransform, titlePosition, 2f));
    }

    private IEnumerator SlideIn(Transform transformSlideObject, float toPosition, float duration)
    {
        var counter = 0f;

        while (counter < duration)
        {
            counter += Time.deltaTime;
            var position = gameTitleTransform.position;
            position +=
                Vector3.down * Mathf.Lerp(position.y, toPosition, counter / duration);
            gameTitleTransform.position = position;

            yield return null;
        }
    }
}