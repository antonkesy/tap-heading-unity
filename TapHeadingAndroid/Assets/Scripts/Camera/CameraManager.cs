using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [Header("Scene")] [SerializeField] private float sceneWidth = 10;
    [Header("Shaking")] [SerializeField] private float shakeDuration = 1f;
    [SerializeField] private float decreaseFactor = 1.0f;
    private float _shakeDuration;
    private Transform _transform;
    [SerializeField] private float shakeAmount = 0.05f;
    private Vector3 _originalPos;
    private bool _isShaking;

    private void Start()
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

        if (Camera.main is { }) Camera.main.orthographicSize = desiredHalfHeight;
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

    internal void StartShaking()
    {
        _shakeDuration = shakeDuration;
        _originalPos = gameObject.transform.position;
        _isShaking = true;
    }
}