using UnityEngine;

namespace TapHeading.Camera
{
    public class CameraManager : MonoBehaviour, ICameraManager
    {
        [Header("Scene")]
        [SerializeField] private float sceneWidth = 10;
        [Header("Shaking")]
        [SerializeField] private float shakeDuration = 1f;
        [SerializeField] private float decreaseFactor = 1.0f;
        [SerializeField] private float shakeAmount = 0.05f;
        
        private float _timeToShakeLeft;
        private Vector3 _originalPos;
        private bool _isShaking;

        private void Awake()
        {
            ScaleCameraToWidth();
        }

        private void Update()
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
            if (_timeToShakeLeft > 0)
            {
                transform.localPosition = _originalPos + Random.insideUnitSphere * shakeAmount;
                _timeToShakeLeft -= decreaseFactor * Time.deltaTime;
            }
            else
            {
                transform.localPosition = _originalPos;
                _isShaking = false;
            }
        }


        public void StartShaking()
        {
            _timeToShakeLeft = shakeDuration;
            _originalPos = transform.position;
            _isShaking = true;
        }
    }
}