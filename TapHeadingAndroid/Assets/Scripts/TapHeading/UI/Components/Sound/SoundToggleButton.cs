using TapHeading.Manager;
using TapHeading.UI.Utility.Transition;
using TapHeading.UI.Utility.Transition.Fade;
using UnityEngine;

namespace TapHeading.UI.Components.Sound
{
    public class SoundToggleButton : MonoBehaviour, ITransition
    {
        [SerializeField] private FaderUI soundOn;
        [SerializeField] private FaderUI soundOff;

        [SerializeField] private ManagerCollector managers;

        private ITransition _currentActive;
        private ITransition _notActive;

        private void Awake()
        {
            _currentActive = soundOn;
            _notActive = soundOff;
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
            if (managers.GetSettings().IsSoundOn())
            {
                _notActive = soundOff;
                _currentActive = soundOn;
            }
            else
            {
                _notActive = soundOn;
                _currentActive = soundOff;
            }

            _notActive.Out();
            _currentActive.In();
        }
    }
}