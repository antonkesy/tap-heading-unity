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
    [Header("Manager")] [SerializeField] private GameManager gameManager;
    [Header("Particles")] [SerializeField] private ParticleSystem thrusterParticleSystem;
    [SerializeField] private ParticleSystem destroyParticleSystem;
    [SerializeField] private GameObject pickupParticleSystemGameObject;
    [SerializeField] private ParticleSystem pickupParticleSystem;
    [Header("Shadow")] [SerializeField] private Transform shadowTransform;
    [SerializeField] private float shadowOffset;

    [Header("Properties")] [SerializeField]
    private float sideSpeed;

    [SerializeField] private float spawnTime;
    private Vector2 _startPosition;
    private Rigidbody2D _rb;
    private bool _isDirectionRight;
    private float _spawnStartPositionY = -20;
    private bool _isDirectionChangeable;
    private bool _isWaitingForRespawn;
    private bool _isRespawning;

    private void Start()
    {
        _startPosition = transform.position;
        _rb = GetComponent<Rigidbody2D>();

        SetSpawnStartPositionY();
    }

    /**
     * Sets spawn y position of player depending on Camera
     */
    private void SetSpawnStartPositionY()
    {
        var mainCam = Camera.main;
        if (mainCam is null) return;
        var frustumHeight = 2.0f * mainCam.orthographicSize *
                            Mathf.Tan(mainCam.fieldOfView * 0.5f * Mathf.Deg2Rad);
        _spawnStartPositionY = frustumHeight * -1f;
    }

    /**
     * Starts player moving in random direction
     */
    internal void StartMoving()
    {
        if (_isRespawning)
        {
            _isWaitingForRespawn = true;
            return;
        }

        ChangeDirection();
        if (Random.Range(0f, 1f) > .5f)
        {
            ChangeDirection();
        }

        thrusterParticleSystem.Play();
    }

    /**
     * Call to change direction return if ChangeDirection is doable
     */
    internal bool CallChangeDirection()
    {
        return ChangeDirection();
    }

    /**
     * Returns if player direction is changeable
     * changes player direction
     */
    private bool ChangeDirection()
    {
        if (!_isDirectionChangeable) return false;
        _isDirectionRight = !_isDirectionRight;
        _rb.velocity = Vector2.zero;
        _rb.AddForce(_isDirectionRight ? Vector2.right * sideSpeed : Vector2.left * sideSpeed, ForceMode2D.Impulse);
        ChangeShadowPosition();
        return true;
    }

    /**
     * Sets player shadow position according to direction is heading to
     */
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

    /**
     * Handles coin collision
     */
    private void OnCoinCollision(GameObject coinGameObject)
    {
        coinGameObject.SetActive(false);
        pickupParticleSystemGameObject.transform.position = coinGameObject.transform.position;
        pickupParticleSystem.Play();
        gameManager.CoinPickedUpCallback();
    }

    /**
    * Destroys Player
    */
    private void DestroyPlayer()
    {
        _rb.velocity = Vector2.zero;
        thrusterParticleSystem.Stop();
        destroyParticleSystem.transform.position = transform.position;
        destroyParticleSystem.Play();
        gameObject.SetActive(false);
        gameManager.DestroyPlayerCallback();
    }

    /*
    * Spawns Player
    */
    internal void SpawnPlayer()
    {
        transform.position = Vector3.up * _spawnStartPositionY;
        gameObject.SetActive(true);
        StartCoroutine(MovePlayerToSpawn());
    }

    /**
     * Moves Player to the spawn 
     */
    private IEnumerator MovePlayerToSpawn()
    {
        _isRespawning = true;
        _isDirectionChangeable = false;
        var time = 0f;
        var targetPosition = Vector2.up * _startPosition;
        _rb.position = Vector2.up * _spawnStartPositionY;
        thrusterParticleSystem.Play();
        var isThrusterStopped = false;
        while (time < spawnTime)
        {
            _rb.MovePosition(Vector2.Lerp(_rb.position, targetPosition, time / spawnTime));
            time += Time.deltaTime;
            if (!isThrusterStopped && Vector2.Distance(_rb.position, targetPosition) <= 0.5f)
            {
                isThrusterStopped = true;
                thrusterParticleSystem.Stop();
            }

            //Stop Lerp early
            if (Vector2.Distance(_rb.position, targetPosition) <= 0.001f)
            {
                break;
            }

            yield return null;
        }

        _rb.position = targetPosition;
        _isDirectionChangeable = true;
        _isRespawning = false;
        if (_isWaitingForRespawn)
        {
            _isWaitingForRespawn = false;
            StartMoving();
        }

        yield return null;
    }
}