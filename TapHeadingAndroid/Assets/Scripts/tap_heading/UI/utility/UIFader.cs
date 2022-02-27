using System.Collections;
using UnityEngine;

namespace tap_heading.UI.utility
{
    /**
 * Lets UIElement Fade in/out via function call
 * Manages visibility
 */
    public class UIFader : MonoBehaviour
    {
        private CanvasGroup _canvasGroup;
        private bool _isFadeIn = true;

        private void Start()
        {
            _canvasGroup = GetComponent<CanvasGroup>();
        }

        /**
     * Starts Fade in/out if not already in wanted fade-state
     */
        public void Fade(bool fadeIn, float duration)
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

        /**
     * Fading of UI-Element by changing canvasGroup-alpha by lerp
     */
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
    }
}