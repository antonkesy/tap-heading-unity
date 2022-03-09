using TapHeading.UI.Utility.Transition;
using TapHeading.UI.Utility.Transition.Zoom;
using UnityEngine;

namespace TapHeading.UI.Components.Text
{
    public class TapInfo : MonoBehaviour, ITransition, IText
    {
        private ShadowText _text;
        private ITransition _zoomUI;

        private void Awake()
        {
            _text = GetComponent<ShadowText>();
            _zoomUI = GetComponent<ZoomUI>();
        }

        public void In()
        {
            _zoomUI.In();
        }

        public void Out()
        {
            _zoomUI.Out();
        }

        public void SetText(string text)
        {
            _text.SetText(text);
        }
    }
}