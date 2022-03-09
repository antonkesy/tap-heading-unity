using TapHeading.UI.Utility.Transition;
using TapHeading.UI.Utility.Transition.Slide;
using UnityEngine;

namespace TapHeading.UI.Components.Title
{
    public class GameTitle : MonoBehaviour, ITransition
    {
        private SliderUI _sliderUI;

        private void Awake()
        {
            _sliderUI = GetComponentInChildren<SliderUI>();
        }

        public void In()
        {
            _sliderUI.In();
        }

        public void Out()
        {
            _sliderUI.Out();
        }
    }
}