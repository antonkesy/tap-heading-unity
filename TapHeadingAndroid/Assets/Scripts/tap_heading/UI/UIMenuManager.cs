using System.Collections;
using System.Collections.Generic;
using tap_heading.UI.utility;
using UnityEngine;

namespace tap_heading.UI
{
    public class UIMenuManager : MonoBehaviour
    {
        [Header("Non-Special Fader")] [SerializeField]
        private UIFader[] serializedFader;

        private readonly List<IFader> faders = new List<IFader>();

        [Header("Game Title")] [SerializeField]
        private UIFader gameTitleFader;

        [SerializeField] private Transform gameTitleTransform;
        private Vector3 _titlePosition;
        [SerializeField] private Transform titleStartTransform;
        [SerializeField] private float titleLerpDuration;
        [SerializeField] private float titleMenuDelay;

        [SerializeField] private float fadeInDuration = .5f;
        [SerializeField] private float fadeOutDuration = .5f;


        [SerializeField] private HighScoreUI highScoreUI;

        [SerializeField] private SoundToggleButton soundButton;

        private void Awake()
        {
            _titlePosition = gameTitleTransform.position;
            foreach (var fader in serializedFader)
            {
                faders.Add(fader);
            }

            faders.Add(soundButton);
        }

        internal void FadeInMenu()
        {
            soundButton.FadeOut(0f);
            FadeAll(true, fadeInDuration);
        }

        internal void FadeOutMenu()
        {
            gameTitleFader.FadeOut(fadeOutDuration);
            FadeAll(false, fadeOutDuration);
            highScoreUI.FadeOut(fadeOutDuration);
        }

        internal void FadeInStart()
        {
            StartCoroutine(WaitForGameTitle());
        }

        private IEnumerator WaitForGameTitle()
        {
            FadeAll(false, 0);
            soundButton.FadeOut(0);
            highScoreUI.FadeOut(0);
            yield return new WaitForSecondsRealtime(titleMenuDelay);
            //Fades in Menu
            FadeAll(true, fadeInDuration);
            yield return null;
        }

        private void FadeAll(bool fadeIn, float duration)
        {
            foreach (var fader in faders)
            {
                if (fadeIn)
                    fader.FadeIn(duration);
                else
                    fader.FadeOut(duration);
            }
        }

        internal void SlideInGameTitle()
        {
            gameTitleTransform.position = titleStartTransform.position;
            gameTitleFader.FadeIn(0);
            StartCoroutine(SlideIn(gameTitleTransform, _titlePosition, titleLerpDuration));
        }

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

        public void SetSound()
        {
            soundButton.Toggle();
        }

        internal void FadeInNewHighScore()
        {
            highScoreUI.FadeIn(fadeInDuration);
        }
    }
}