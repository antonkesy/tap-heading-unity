using tap_heading.UI.utility;
using tap_heading.UI.utility.Fade;
using TMPro;
using UnityEngine;

namespace tap_heading.UI.components.Text
{
    public class ShadowText : MonoBehaviour, IText, IFader
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

        public void FadeIn(float duration)
        {
            foreach (var fader in _faders)
            {
                fader.FadeIn(duration);
            }
        }

        public void FadeOut(float duration)
        {
            foreach (var fader in _faders)
            {
                fader.FadeOut(duration);
            }
        }

        public void SetActive(bool active)
        {
            foreach (var uiFader in _faders)
            {
                uiFader.gameObject.SetActive(active);
            }
        }
    }
}