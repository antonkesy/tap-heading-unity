using System;
using tap_heading.Settings;
using tap_heading.UI.utility;
using UnityEngine;

namespace tap_heading.UI.components.sound
{
    public class SoundToggleButton : MonoBehaviour, ISoundButton
    {
        [SerializeField] private UIFader soundOnFader;
        [SerializeField] private UIFader soundOffFader;

        [SerializeField] private PlayerPrefsManager settings;

        private IFader _currentActive;

        private void Awake()
        {
            _currentActive = soundOnFader;
        }

        private void Start()
        {
            soundOffFader.FadeOut(0f);
            soundOnFader.FadeOut(0f);
            _currentActive = settings.IsSoundOn() ? soundOnFader : soundOffFader;
        }

        public void FadeIn(float duration)
        {
            _currentActive.FadeIn(duration);
        }

        public void FadeOut(float duration)
        {
            _currentActive.FadeOut(duration);
        }

        public void Toggle()
        {
            if (settings.IsSoundOn())
            {
                soundOffFader.FadeOut(0);
                soundOnFader.FadeIn(0);
                _currentActive = soundOnFader;
            }
            else
            {
                soundOffFader.FadeIn(0);
                soundOnFader.FadeOut(0);
                _currentActive = soundOffFader;
            }

            _currentActive.FadeIn(0f);
        }
    }
}