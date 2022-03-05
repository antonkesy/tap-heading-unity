using System.Collections;
using System.Collections.Generic;
using tap_heading.UI.components.highscore;
using tap_heading.UI.components.sound;
using tap_heading.UI.components.Title;
using tap_heading.UI.utility;
using UnityEngine;

namespace tap_heading.UI
{
    public class UIMenuManager : MonoBehaviour
    {
        [SerializeField] private float titleMenuDelay;
        [SerializeField] private UIFader[] serializedFader;

        private readonly List<IFader> _faders = new List<IFader>();

        [SerializeField] private float fadeInDuration = .5f;
        [SerializeField] private GameTitle gameTitle;
        [SerializeField] private float fadeOutDuration = .5f;


        [SerializeField] private HighScoreUI highScoreUI;

        [SerializeField] private SoundToggleButton soundButton;

        private void Awake()
        {
            foreach (var fader in serializedFader)
            {
                _faders.Add(fader);
            }

            _faders.Add(soundButton);
            _faders.Add(highScoreUI);
            _faders.Add(gameTitle);
        }

        internal void FadeInMenu()
        {
            FadeAll(true, fadeInDuration);
        }

        internal void FadeOutMenu()
        {
            gameTitle.FadeOut(fadeOutDuration);
            FadeAll(false, fadeOutDuration);
        }

        internal void FadeInStart()
        {
            StartCoroutine(WaitForGameTitle());
        }

        private IEnumerator WaitForGameTitle()
        {
            FadeAll(false, 0);
            gameTitle.SlideIn();
            yield return new WaitForSecondsRealtime(titleMenuDelay);
            FadeAll(true, fadeInDuration);
            yield return null;
        }

        private void FadeAll(bool fadeIn, float duration)
        {
            foreach (var fader in _faders)
            {
                if (fadeIn)
                    fader.FadeIn(duration);
                else
                    fader.FadeOut(duration);
            }
        }


        public void SetSound()
        {
            soundButton.Update();
        }

        internal void FadeInNewHighScore()
        {
            highScoreUI.FadeInNewHighScore(fadeInDuration);
        }
    }
}