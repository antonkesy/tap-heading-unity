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

using System.Collections;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    [SerializeField] private Vector2 startPosition;
    [SerializeField] private ParticleSystem thrusterParticleSystem;
    [SerializeField] private ParticleSystem destroyParticleSystem;
    [SerializeField] private GameObject pickupParticleSystemGameObject;
    [SerializeField] private ParticleSystem pickupParticleSystem;

    [SerializeField] private Transform shadowTransform;
    [SerializeField] private float shadowOffset;


    private GameManager _gameManager;

    [SerializeField] private float sideSpeed;

    private Rigidbody2D _rb;

    private bool _isDirectionRight;

    private float _spawnStartPositionY = -20;

    [SerializeField] private float spawnTime;

    private bool _isDirectionChangeable;

    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();

        SetSpawnStartPositionY();
    }

    private void SetSpawnStartPositionY()
    {
        var mainCam = Camera.main;
        if (mainCam is { })
        {
            var frustumHeight = 2.0f * mainCam.orthographicSize *
                                Mathf.Tan(mainCam.fieldOfView * 0.5f * Mathf.Deg2Rad);
            _spawnStartPositionY = (frustumHeight) * -1f;
        }
    }

    internal void StartMoving()
    {
        ChangeDirection();
        if (Random.Range(0f, 1f) > .5f)
        {
            ChangeDirection();
        }

        thrusterParticleSystem.Play();
    }

    internal bool CallChangeDirection()
    {
        return ChangeDirection();
    }

    private bool ChangeDirection()
    {
        if (!_isDirectionChangeable) return false;
        _isDirectionRight = !_isDirectionRight;
        _rb.velocity = Vector2.zero;
        _rb.AddForce(_isDirectionRight ? Vector2.right * sideSpeed : Vector2.left * sideSpeed, ForceMode2D.Impulse);
        ChangeShadowPosition();
        return true;
    }

    private void ChangeShadowPosition()
    {
        shadowTransform.localPosition = Vector3.right * (_isDirectionRight ? -shadowOffset : shadowOffset) +
                                        Vector3.up * -shadowOffset;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        var og = other.gameObject;
        if (og.CompareTag("Wall") || og.CompareTag("Bar"))
        {
            DestroyPlayer();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        var og = other.gameObject;
        if (og.CompareTag("Coin"))
        {
            OnCoinCollision(og);
        }
    }

    private void OnCoinCollision(GameObject coinGameObject)
    {
        coinGameObject.SetActive(false);
        pickupParticleSystemGameObject.transform.position = coinGameObject.transform.position;
        pickupParticleSystem.Play();
        _gameManager.CoinPickedUpCallback();
    }

    private void DestroyPlayer()
    {
        _rb.velocity = Vector2.zero;
        thrusterParticleSystem.Stop();
        destroyParticleSystem.transform.position = transform.position;
        destroyParticleSystem.Play();
        gameObject.SetActive(false);
        _gameManager.DestroyPlayerCallback();
    }

    internal void SetManager(GameManager gameManager)
    {
        _gameManager = gameManager;
    }

    internal void SpawnPlayer()
    {
        transform.position = Vector3.up * _spawnStartPositionY;
        gameObject.SetActive(true);
        StartCoroutine(MovePlayerToSpawn());
    }

    private IEnumerator MovePlayerToSpawn()
    {
        _isDirectionChangeable = false;
        var time = 0f;
        var targetPosition = Vector2.up * startPosition;
        _rb.position = Vector2.up * _spawnStartPositionY;
        thrusterParticleSystem.Play();
        while (time < spawnTime)
        {
            _rb.MovePosition(Vector2.Lerp(_rb.position, targetPosition, time / spawnTime));
            time += Time.deltaTime;
            if (Vector2.Distance(_rb.position, targetPosition) <= 0.5f)
            {
                thrusterParticleSystem.Stop();
            }

            if (Vector2.Distance(_rb.position, targetPosition) <= 0.05f)
            {
                break;
            }

            yield return null;
        }

        _rb.position = targetPosition;
        _isDirectionChangeable = true;

        yield return null;
    }
}