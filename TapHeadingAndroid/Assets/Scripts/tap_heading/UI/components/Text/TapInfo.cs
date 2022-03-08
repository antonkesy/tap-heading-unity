using tap_heading.UI.utility.Transition;
using tap_heading.UI.utility.Zoom;
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
            _zoom = GetComponent<IZoom>();
        }

        public void In()
        {
            _zoom.ZoomIn();
        }

        public void Out()
        {
            _zoom.ZoomOut();
        }

        public void SetText(string text)
        {
            _text.SetText(text);
        }
    }
}