using System.Collections;
using UnityEngine;

namespace tap_heading.UI.utility.Transition.Fade
{
    public class UIFader : MonoBehaviour, ITransition
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
            _fading = StartCoroutine(DoFade(fadeIn ? 1 : 0, fadeIn,
                fadeIn ? fadeInDuration : fadeOutDuration));
        }

        private IEnumerator DoFade(float end, bool endState, float duration)
        {
            var counter = 0f;
            var start = _canvasGroup.alpha;

            while (counter < duration)
            {
                counter += Time.deltaTime;
                _canvasGroup.alpha = Mathf.Lerp(start, end, counter / duration);

                yield return null;
            }

            gameObject.SetActive(endState);
            _fading = null;
        }

        public void In()
        {
            Fade(true);
        }

        public void Out()
        {
            Fade(false);
        }
    }
}