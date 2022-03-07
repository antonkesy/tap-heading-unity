using System.Collections;
using UnityEngine;

namespace tap_heading.UI.utility.Fade
{
    public class UIFader : MonoBehaviour, IFader
    {
        private CanvasGroup _canvasGroup;
        private Coroutine _fading;

        private void Awake()
        {
            _canvasGroup = GetComponent<CanvasGroup>();
        }

        private void Fade(bool fadeIn, float duration)
        {
            if (_fading != null)
            {
                StopCoroutine(_fading);
            }

            gameObject.SetActive(true);
            _fading = StartCoroutine(DoFade(fadeIn ? 0 : 1, fadeIn ? 1 : 0, fadeIn, duration));
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
            _fading = null;
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