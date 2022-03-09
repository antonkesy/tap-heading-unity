using System.Collections;
using UnityEngine;

namespace TapHeading.UI.Utility.Transition.Slide
{
    public class SliderUI : MonoBehaviour, ITransition
    {
        [SerializeField] protected float slideDuration;
        [SerializeField] protected Vector3 hidePosition;
        private Vector3 _showPosition;
        private Coroutine _slide;

        private void Awake()
        {
            _showPosition = gameObject.transform.localPosition;
            transform.localPosition = hidePosition;
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

        public void In()
        {
            Slide(true);
        }

        public void Out()
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

            _slide = null;
        }
    }
}