using System.Collections.Generic;
using TapHeading.UI.Components.HighScore;
using TapHeading.UI.Components.Sound;
using TapHeading.UI.Components.Text;
using TapHeading.UI.Utility.Transition;
using TapHeading.UI.Utility.Transition.Fade;
using UnityEngine;

namespace TapHeading.UI
{
    public class UIMenuManager : MonoBehaviour, ITransition
    {
        [SerializeField] private FaderUI[] serializedFader;

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