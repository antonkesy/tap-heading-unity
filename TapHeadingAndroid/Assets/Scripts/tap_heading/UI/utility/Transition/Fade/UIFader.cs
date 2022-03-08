using System.Collections;
using UnityEngine;

namespace tap_heading.UI.utility.Transition.Fade
{
    public class UIFader : MonoBehaviour, IFader, ITransition
    {
        [SerializeField] private float fadeInDuration;
        [SerializeField] private float fadeOutDuration;
        private CanvasGroup _canvasGroup;
        private Coroutine _fading;

        private void Awake()
        {
            _canvasGroup = GetComponent<CanvasGroup>();
            _canvasGroup.alpha = 0f;
        }

        private void Fade(bool fadeIn)
        {
            if (_fading != null)
            {
                StopCoroutine(_fading);
            }

            gameObject.SetActive(true);
            _fading = StartCoroutine(DoFade(fadeIn ? 0 : 1, fadeIn ? 1 : 0, fadeIn,
                fadeIn ? fadeInDuration : fadeOutDuration));
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

        public void FadeIn()
        {
            Fade(true);
        }

        public void FadeOut()
        {
            Fade(false);
        }

        public void In()
        {
            FadeIn();
        }

        public void Out()
        {
            FadeOut();
        }
    }
}