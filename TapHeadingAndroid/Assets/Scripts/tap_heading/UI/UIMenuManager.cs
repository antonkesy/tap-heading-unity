using System.Collections.Generic;
using tap_heading.UI.components.highscore;
using tap_heading.UI.components.sound;
using tap_heading.UI.utility;
using UnityEngine;

namespace tap_heading.UI
{
    public class UIMenuManager : MonoBehaviour, IFader
    {
        [SerializeField] private UIFader[] serializedFader;

        private readonly List<IFader> _faders = new List<IFader>();

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
        }

        public void SetSound()
        {
            soundButton.Toggle();
        }

        internal void FadeInNewHighScore(float duration)
        {
            highScoreUI.FadeInNewHighScore(duration);
        }

        public void FadeIn(float duration)
        {
            foreach (var fader in _faders)
            {
                fader.FadeIn(duration);
            }
        }

        public void FadeOut(float duration)
        {
            foreach (var fader in _faders)
            {
                fader.FadeOut(duration);
            }
        }
    }
}