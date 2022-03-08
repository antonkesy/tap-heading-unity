using System;
using tap_heading.Settings;
using tap_heading.UI.utility;
using tap_heading.UI.utility.Transition;
using tap_heading.UI.utility.Transition.Fade;
using UnityEngine;

namespace tap_heading.UI.components.sound
{
    public class SoundToggleButton : MonoBehaviour, ITransition
    {
        [SerializeField] private UIFader soundOnFader;
        [SerializeField] private UIFader soundOffFader;

        [SerializeField] private PlayerPrefsManager settings;

        private IFader _currentActive;
        private IFader _notActive;

        private void Awake()
        {
            _currentActive = soundOnFader;
            Toggle();
        }

        public void In()
        {
            _currentActive.FadeIn();
            _notActive.FadeOut();
        }

        public void Out()
        {
            _currentActive.FadeOut();
            _notActive.FadeOut();
        }

        public void Toggle()
        {
            if (settings.IsSoundOn())
            {
                _notActive = soundOffFader;
                _currentActive = soundOnFader;
            }
            else
            {
                _notActive = soundOnFader;
                _currentActive = soundOffFader;
            }

            _notActive.FadeOut();
            _currentActive.FadeIn();
        }
    }
}