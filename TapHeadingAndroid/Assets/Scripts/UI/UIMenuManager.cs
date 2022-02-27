using System.Collections;
using UnityEngine;

/**
 * UI Game Menu Manager
 */
public class UIMenuManager : MonoBehaviour
{
    [Header("Non-Special Fader")] [SerializeField]
    private UIFader[] faders;

    [Header("Game Title")] [SerializeField]
    private UIFader gameTitleFader;

    [SerializeField] private Transform gameTitleTransform;
    private Vector3 _titlePosition;
    [SerializeField] private Transform titleStartTransform;
    [SerializeField] private float titleLerpDuration;
    [SerializeField] private float titleMenuDelay;

    [SerializeField] private float fadeInDuration = .5f;
    [SerializeField] private float fadeOutDuration = .5f;

    [Header("Sound Buttons")] [SerializeField]
    private UIFader soundOnFader;

    [SerializeField] private UIFader soundOffFader;
    private bool _isSoundOff;

    [Header("New HighScore")] [SerializeField]
    private UIFader newHighScoreFader;

    private void Start()
    {
        _titlePosition = gameTitleTransform.position;
    }

    /**
     * Fades In Game Menu
     */
    internal void FadeInMenu()
    {
        soundOffFader.Fade(false, 0);
        soundOnFader.Fade(false, 0);
        FadeAll(true, fadeInDuration);
        FadeInSound();
    }

    /**
     * Fades Out Game Menu
     */
    internal void FadeOutMenu()
    {
        gameTitleFader.Fade(false, fadeOutDuration);
        FadeAll(false, fadeOutDuration);
        newHighScoreFader.Fade(false, fadeOutDuration);
        if (_isSoundOff)
        {
            soundOffFader.Fade(false, 0f);
            soundOnFader.Fade(false, fadeOutDuration);
        }
        else
        {
            soundOnFader.Fade(false, 0f);
            soundOffFader.Fade(false, fadeOutDuration);
        }
    }

    /**
     * Fades In Start Menu
     */
    internal void FadeInStart()
    {
        StartCoroutine(WaitForGameTitle());
    }

    /**
     * Waits for Game Title to slides in, then starts fading in Start Menu
     */
    private IEnumerator WaitForGameTitle()
    {
        FadeAll(false, 0);
        soundOffFader.Fade(false, 0);
        soundOnFader.Fade(false, 0);
        newHighScoreFader.Fade(false, 0);
        yield return new WaitForSecondsRealtime(titleMenuDelay);
        //Fades in Menu
        FadeAll(true, fadeInDuration);
        FadeInSound();
        yield return null;
    }

    /**
     * Iterates over all unspecific fader and fades them
     */
    private void FadeAll(bool fadeIn, float duration)
    {
        foreach (var fader in faders)
        {
            fader.Fade(fadeIn, duration);
        }
    }

    /**
     * Fades sound depending if sound on/off
     */
    private void FadeInSound()
    {
        if (_isSoundOff)
        {
            soundOffFader.Fade(false, 0f);
            soundOnFader.Fade(true, fadeInDuration);
        }
        else
        {
            soundOnFader.Fade(false, 0f);
            soundOffFader.Fade(true, fadeInDuration);
        }
    }

    /**
     * Starts Game Title slide in
     */
    internal void SlideInGameTitle()
    {
        gameTitleTransform.position = titleStartTransform.position;
        gameTitleFader.Fade(true, 0);
        StartCoroutine(SlideIn(gameTitleTransform, _titlePosition, titleLerpDuration));
    }

    /**
     * Slides in GameTitle
     */
    private static IEnumerator SlideIn(Transform transformSlideObject, Vector3 toPosition, float duration)
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

    /**
     * Sets _isSoundOff Flag and fades in/out buttons
     */
    internal void SetSound(bool isSoundOff)
    {
        _isSoundOff = isSoundOff;
        soundOffFader.Fade(!_isSoundOff, 0f);
        soundOnFader.Fade(_isSoundOff, 0f);
    }

    /**
     * Fades in new highScore text
     */
    internal void FadeInNewHighScore()
    {
        newHighScoreFader.Fade(true, fadeInDuration);
    }
}