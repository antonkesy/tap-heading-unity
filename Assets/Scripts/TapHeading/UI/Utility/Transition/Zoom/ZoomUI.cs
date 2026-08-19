using System.Collections;
using UnityEngine;

namespace TapHeading.UI.Utility.Transition.Zoom
{
    public class ZoomUI : MonoBehaviour, ITransition
    {
        [SerializeField]
        private float zoomDuration;
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

            Vector3 to;
            if (zoomIn)
            {
                to = _originalScale;
            }
            else
            {
                to = Vector3.zero;
            }

            gameObject.SetActive(true);
            _zoom = StartCoroutine(Zooming(to, zoomIn));
        }

        private IEnumerator Zooming(Vector3 to, bool endState)
        {
            var counter = 0f;
            //lerping from the live scale never reaches `to`, so it drifts every cycle
            var start = transform.localScale;

            while (counter < zoomDuration)
            {
                counter += Time.deltaTime;
                transform.localScale = Vector3.Lerp(start, to, counter / zoomDuration);

                yield return null;
            }

            transform.localScale = to;
            gameObject.SetActive(endState);
            _zoom = null;
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
