using tap_heading.UI.components.Text;
using tap_heading.UI.utility.Transition;
using tap_heading.UI.utility.Transition.Slide;
using UnityEngine;

namespace tap_heading.UI.components.Title
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