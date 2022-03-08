using System.Collections.Generic;
using tap_heading.UI.components.highscore;
using tap_heading.UI.components.sound;
using tap_heading.UI.components.Text;
using tap_heading.UI.utility.Transition;
using tap_heading.UI.utility.Transition.Fade;
using UnityEngine;

namespace tap_heading.UI
{
    public class UIMenuManager : MonoBehaviour, ITransition
    {
        [SerializeField] private UIFader[] serializedFader;

        private readonly List<ITransition> _transitions = new List<ITransition>();

        [SerializeField] private HighScoreUI highScoreUI;

        [SerializeField] private SoundToggleButton soundButton;

        [SerializeField] private TapInfo tapToInfo;

        private void Awake()
        {
            foreach (var fader in serializedFader)
            {
                _transitions.Add(fader);
            }

            _transitions.Add(soundButton);
            _transitions.Add(highScoreUI);
            _transitions.Add(tapToInfo);
        }

        internal void FadeInNewHighScore(float duration)
        {
            highScoreUI.FadeInNewHighScore(duration);
        }

        public void In()
        {
            soundButton.Toggle();
            foreach (var trans in _transitions)
            {
                trans.In();
            }
        }

        public void Out()
        {
            foreach (var trans in _transitions)
            {
                trans.Out();
            }
        }
    }
}