using tap_heading.UI.utility.Transition;
using tap_heading.UI.utility.Transition.Zoom;
using UnityEngine;

namespace tap_heading.UI.components.Text
{
    public class TapInfo : MonoBehaviour, ITransition, IText
    {
        private ShadowText _text;
        private IZoom _zoom;

        private void Awake()
        {
            _text = GetComponent<ShadowText>();
            _zoom = GetComponent<ZoomUI>();
        }

        public void In()
        {
            _zoom.In();
        }

        public void Out()
        {
            _zoom.Out();
        }

        public void SetText(string text)
        {
            _text.SetText(text);
        }
    }
}