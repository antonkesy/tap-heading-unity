using System.Collections;
using UnityEngine;

namespace TapHeading.UI.Utility.Transition.Zoom
{
    public class ZoomUI : MonoBehaviour, ITransition
    {
        [SerializeField] private float zoomDuration;
        private Vector3 _originalScale;
        private Coroutine _zoom;

        private void Awake()
        {
            var trans = transform;
            _originalScale = trans.localScale;
            trans.localScale = Vector3.zero;
        }

        private void Zoom(bool zoomIn)
        {
            if (_zoom != null)
            {
                StopCoroutine(_zoom);
            }

            Vector3 from;
            Vector3 to;
            if (zoomIn)
            {
                from = Vector3.zero;
                to = _originalScale;
            }
            else
            {
                from = _originalScale;
                to = Vector3.zero;
            }

            gameObject.SetActive(true);
            _zoom = StartCoroutine(Zooming(from, to));
        }

        private IEnumerator Zooming(Vector3 from, Vector3 to)
        {
            var counter = 0f;

            while (counter < zoomDuration)
            {
                counter += Time.deltaTime;
                var newScale = transform.localScale;
                newScale = Vector3.Lerp(newScale, to, counter / zoomDuration);
                transform.localScale = newScale;

                yield return null;
            }

            _zoom = null;
            yield return null;
        }

        public void In()
        {
            Zoom(true);
        }

        public void Out()
        {
            Zoom(false);
        }
    }
}