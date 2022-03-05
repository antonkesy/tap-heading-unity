using System.Collections;
using tap_heading.UI.utility;
using UnityEngine;

namespace tap_heading.UI.components.Title
{
    public class GameTitle : MonoBehaviour, IFader
    {
        private UIFader _gameTitleFader;

        [SerializeField] private float titleLerpDuration;
        [SerializeField] private Transform titleStartTransform;

        private void Awake()
        {
            _gameTitleFader = GetComponent<UIFader>();
        }

        public void FadeIn(float duration)
        {
            _gameTitleFader.FadeIn(duration);
        }

        public void FadeOut(float duration)
        {
            _gameTitleFader.FadeOut(duration);
        }

        public void SlideIn(IGameTitleListener listener)
        {
            var originalPosition = transform.position;
            transform.position = titleStartTransform.position;
            _gameTitleFader.FadeIn(0);
            StartCoroutine(SlideIn(transform, originalPosition, titleLerpDuration, listener));
        }

        private static IEnumerator SlideIn(Transform transformSlideObject, Vector3 toPosition, float duration,
            IGameTitleListener listener)
        {
            var counter = 0f;

            while (counter < duration)
            {
                counter += Time.deltaTime;
                var position = transformSlideObject.position;
                position = Vector3.Lerp(position, toPosition, counter / duration);
                transformSlideObject.position = position;

                yield return null;
            }

            listener?.OnSlideInDone();
        }
    }
}