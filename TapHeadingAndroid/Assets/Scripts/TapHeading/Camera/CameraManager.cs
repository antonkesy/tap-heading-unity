using UnityEngine;

namespace TapHeading.Camera
{
    public class CameraManager : MonoBehaviour, ICameraManager
    {
        [Header("Scene")] [SerializeField] private float sceneWidth = 10;
        [Header("Shaking")] [SerializeField] private float shakeDuration = 1f;
        [SerializeField] private float decreaseFactor = 1.0f;
        private float _shakeDuration;
        private Transform _transform;
        [SerializeField] private float shakeAmount = 0.05f;
        private Vector3 _originalPos;
        private bool _isShaking;

        private void Awake()
        {
            _transform = GetComponent<Transform>();
            ScaleCameraToWidth();
        }

        private void LateUpdate()
        {
            if (_isShaking)
            {
                Shake();
            }
        }

        private void ScaleCameraToWidth()
        {
            var unitsPerPixel = sceneWidth / Screen.width;

            var desiredHalfHeight = 0.5f * unitsPerPixel * Screen.height;

            if (UnityEngine.Camera.main is { }) UnityEngine.Camera.main.orthographicSize = desiredHalfHeight;
        }

        private void Shake()
        {
            if (_shakeDuration > 0)
            {
                _transform.localPosition = _originalPos + Random.insideUnitSphere * shakeAmount;
                _shakeDuration -= decreaseFactor * Time.deltaTime;
            }
            else
            {
                _transform.localPosition = _originalPos;
                _isShaking = false;
            }
        }


        public void StartShaking()
        {
            _shakeDuration = shakeDuration;
            _originalPos = gameObject.transform.position;
            _isShaking = true;
        }
    }
}