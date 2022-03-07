using System.Collections;
using UnityEngine;

namespace tap_heading.UI.utility.Slide
{
    public class Slider : MonoBehaviour, ISlider
    {
        [SerializeField] protected float slideDuration;
        [SerializeField] protected Vector3 hidePosition;
        private Vector3 _showPosition;
        private Coroutine _slide;

        private void Awake()
        {
            _showPosition = gameObject.transform.localPosition;
            gameObject.transform.localPosition = hidePosition;
        }

        private void Slide(bool slideIn)
        {
            gameObject.SetActive(true);
            if (_slide != null)
            {
                StopCoroutine(_slide);
            }

            _slide = StartCoroutine(
                Slide(slideIn ? _showPosition : hidePosition));
        }

        public void SlideIn()
        {
            Slide(true);
        }

        public void SlideOut()
        {
            Slide(false);
        }

        private IEnumerator Slide(Vector3 toPosition)
        {
            var counter = 0f;

            while (counter < slideDuration)
            {
                counter += Time.deltaTime;
                var position = transform.localPosition;
                position = Vector3.Lerp(position, toPosition, counter / slideDuration);
                transform.localPosition = position;

                yield return null;
            }
        }
    }
}