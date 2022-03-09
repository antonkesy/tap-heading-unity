using tap_heading.Settings;
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

        private ITransition _currentActive;
        private ITransition _notActive;

        private void Awake()
        {
            _currentActive = soundOnFader;
            _notActive = soundOffFader;
        }

        public void In()
        {
            _currentActive.In();
            _notActive.Out();
        }

        public void Out()
        {
            _currentActive.Out();
            _notActive.Out();
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

            _notActive.Out();
            _currentActive.In();
        }
    }
}