using tap_heading.UI.components.Text;
using tap_heading.UI.utility.Fade;
using tap_heading.UI.utility.Slide;
using UnityEngine;

namespace tap_heading.UI.components.Title
{
    public class GameTitle : MonoBehaviour, ISlider, IFader
    {
        private ShadowText _text;
        private Slider _slider;

        private void Awake()
        {
            _text = GetComponentInChildren<ShadowText>();
            _slider = GetComponentInChildren<Slider>();
        }

        public void FadeIn(float duration)
        {
            _text.FadeIn(duration);
        }

        public void FadeOut(float duration)
        {
            _text.FadeOut(duration);
        }

        public void SlideIn()
        {
            _slider.SlideIn();
        }

        public void SlideOut()
        {
            _slider.SlideOut();
        }
    }
}