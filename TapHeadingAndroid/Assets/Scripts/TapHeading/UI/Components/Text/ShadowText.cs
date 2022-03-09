using TapHeading.UI.Utility.Transition;
using TMPro;
using UnityEngine;

namespace TapHeading.UI.Components.Text
{
    public class ShadowText : MonoBehaviour, ITransition
    {
        private TextMeshProUGUI[] _texts;
        private ITransition[] _faders;

        private void Awake()
        {
            _texts = GetComponentsInChildren<TextMeshProUGUI>();
            _faders = GetComponentsInChildren<ITransition>();
        }

        public void SetText(string text)
        {
            foreach (var texts in _texts)
            {
                texts.text = text;
            }
        }

        public void In()
        {
            foreach (var fader in _faders)
            {
                if (fader.Equals(this)) continue;
                fader.In();
            }
        }

        public void Out()
        {
            foreach (var fader in _faders)
            {
                if (fader.Equals(this)) continue;
                fader.Out();
            }
        }
    }
}