using TapHeading.UI.Components.Text;
using TapHeading.UI.Utility.Transition;
using TapHeading.UI.Utility.Transition.Slide;
using UnityEngine;

namespace TapHeading.UI.Components.Title
{
    public class GameTitle : MonoBehaviour, ITransition
    {
        private ShadowText _text;
        private Slider _slider;

        private void Awake()
        {
            _text = GetComponentInChildren<ShadowText>();
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