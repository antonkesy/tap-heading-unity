using System;
using tap_heading.Settings;
using tap_heading.UI.utility;
using tap_heading.UI.utility.Fade;
using UnityEngine;

namespace tap_heading.UI.components.sound
{
    public class SoundToggleButton : MonoBehaviour, ISoundButton
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

        public void FadeIn(float duration)
        {
            _currentActive.FadeIn(duration);
            _notActive.FadeOut(0f);
        }

        public void FadeOut(float duration)
        {
            _currentActive.FadeOut(duration);
            _notActive.FadeOut(0f);
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

            _notActive.FadeOut(0f);
            _currentActive.FadeIn(0f);
        }
    }
}