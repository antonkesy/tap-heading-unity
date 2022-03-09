using TapHeading.UI.Utility.Transition;
using TapHeading.UI.Utility.Transition.Zoom;
using UnityEngine;

namespace TapHeading.UI.Components.Text
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