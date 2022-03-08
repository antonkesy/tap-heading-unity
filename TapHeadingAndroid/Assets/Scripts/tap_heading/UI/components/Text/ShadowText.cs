using tap_heading.UI.utility.Transition;
using tap_heading.UI.utility.Transition.Fade;
using TMPro;
using UnityEngine;

namespace tap_heading.UI.components.Text
{
    public class ShadowText : MonoBehaviour, ITransition
    {
        private TextMeshProUGUI[] _texts;
        private UIFader[] _faders;

        private void Awake()
        {
            _texts = GetComponentsInChildren<TextMeshProUGUI>();
            _faders = GetComponentsInChildren<UIFader>();
        }

        public void SetText(string text)
        {
            foreach (var texts in _texts)
            {
                texts.text = text;
            }
        }

        public void SetActive(bool active)
        {
            foreach (var uiFader in _faders)
            {
                uiFader.gameObject.SetActive(active);
            }
        }

        public void In()
        {
            foreach (var fader in _faders)
            {
                fader.FadeIn();
            }
        }

        public void Out()
        {
            foreach (var fader in _faders)
            {
                fader.FadeOut();
            }
        }
    }
}