using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField] private float sceneWidth = 10;

    private Transform _transform;

    [SerializeField] private float shakeDuration = 1f;

    public float shakeAmount = 0.05f;
    [SerializeField] private float decreaseFactor = 1.0f;

    private Vector3 _originalPos;

    private bool _isShaking;

    void Start()
    {
        _transform = GetComponent<Transform>();
        ScaleCameraToWidth();
    }

    // Update is called once per frame
    void LateUpdate()
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
        if (shakeDuration > 0)
        {
            _transform.localPosition = _originalPos + Random.insideUnitSphere * shakeAmount;
            shakeDuration -= decreaseFactor * Time.deltaTime;
        }
        else
        {
            shakeDuration = 0f;
            _transform.localPosition = _originalPos;
            _isShaking = false;
        }
    }

    internal void StartShaking()
    {
        _originalPos = gameObject.transform.position;
        _isShaking = true;
    }
}