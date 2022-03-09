using TapHeading.UI.Utility.Transition;
using TapHeading.UI.Utility.Transition.Slide;
using UnityEngine;

namespace TapHeading.UI.Components.Title
{
    public class GameTitle : MonoBehaviour, ITransition
    {
        private Slider _slider;

        private void Awake()
        {
            _slider = GetComponentInChildren<Slider>();
        }

        public void In()
        {
            _slider.In();
        }

        public void Out()
        {
            _slider.Out();
        }
    }
}