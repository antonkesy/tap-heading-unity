/*
MIT License

Copyright (c) 2021 Anton Kesy

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
 */
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField] private float sceneWidth = 10;

    private Transform _transform;

    [SerializeField] private float shakeDuration = 1f;
    private float _shakeDuration;

    public float shakeAmount = 0.05f;
    [SerializeField] private float decreaseFactor = 1.0f;

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