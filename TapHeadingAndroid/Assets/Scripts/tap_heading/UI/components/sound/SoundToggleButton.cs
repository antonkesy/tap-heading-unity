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

        private UIFader _currentActive;

        private void Start()
        {
            Update();
        }

        public void FadeIn(float duration)
        {
            _currentActive.FadeIn(duration);
        }

        public void FadeOut(float duration)
        {
            _currentActive.FadeOut(duration);
        }

        public void Update()
        {
            soundOffFader.gameObject.SetActive(!settings.IsSoundOn());
            soundOnFader.gameObject.SetActive(settings.IsSoundOn());
            _currentActive = settings.IsSoundOn() ? soundOnFader : soundOffFader;
        }
    }
}