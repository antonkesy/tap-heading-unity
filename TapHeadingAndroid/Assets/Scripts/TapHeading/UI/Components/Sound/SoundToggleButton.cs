using TapHeading.Manager;
using TapHeading.UI.Utility.Transition;
using TapHeading.UI.Utility.Transition.Fade;
using UnityEngine;

namespace TapHeading.UI.Components.Sound
{
    public class SoundToggleButton : MonoBehaviour, ITransition
    {
        [SerializeField] private UIFader soundOnFader;
        [SerializeField] private UIFader soundOffFader;

        [SerializeField] private ManagerCollector managers;

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
            if (managers.GetSettings().IsSoundOn())
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