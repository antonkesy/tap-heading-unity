using System.Collections;
using UnityEngine;

namespace tap_heading.UI.utility.Fade
{
    public class UIFader : MonoBehaviour, IFader
    {
        private CanvasGroup _canvasGroup;
        private bool _isFadeIn = true;

        private void Awake()
        {
            _canvasGroup = GetComponent<CanvasGroup>();
        }

        private void Fade(bool fadeIn, float duration)
        {
            //Checks if already is faded 
            if (_isFadeIn == fadeIn)
            {
                return;
            }

            _isFadeIn = fadeIn;

            gameObject.SetActive(true);
            StartCoroutine(DoFade(fadeIn ? 0 : 1, fadeIn ? 1 : 0, fadeIn, duration));
        }

        private IEnumerator DoFade(float start, float end, bool endState, float duration)
        {
            var counter = 0f;

            while (counter < duration)
            {
                counter += Time.deltaTime;
                _canvasGroup.alpha = Mathf.Lerp(start, end, counter / duration);

                yield return null;
            }

            gameObject.SetActive(endState);
        }

        public void FadeIn(float duration)
        {
            Fade(true, duration);
        }

        public void FadeOut(float duration)
        {
            Fade(false, duration);
        }
    }
}