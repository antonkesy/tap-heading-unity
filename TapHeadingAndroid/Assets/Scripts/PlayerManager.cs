using System;
using UnityEngine;
using Random = System.Random;

public class PlayerManager : MonoBehaviour
{
    [SerializeField] private Vector2 startPosition;
    [SerializeField] private ParticleSystem thrusterParticleSystem;
    [SerializeField] private ParticleSystem destroyParticleSystem;

    private GameManager _gameManager;

    [SerializeField] private float sideSpeed;

    private Rigidbody2D _rb;

    private bool _isDirectionRight;

    private Vector2 _pauseVelocity;

    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    internal void StartMoving()
    {
        ChangeDirection();
        if (UnityEngine.Random.Range(0f, 1f) > .5f)
        {
            ChangeDirection();
        }

        thrusterParticleSystem.Play();
    }

    internal void CallChangeDirection()
    {
        ChangeDirection();
    }

    private void ChangeDirection()
    {
        _isDirectionRight = !_isDirectionRight;
        _rb.velocity = Vector2.zero;
        _rb.AddForce(_isDirectionRight ? Vector2.right * sideSpeed : Vector2.left * sideSpeed, ForceMode2D.Impulse);
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
        _gameManager.CoinPickedUpCallback();
    }

    private void DestroyPlayer()
    {
        _rb.velocity = Vector2.zero;
        thrusterParticleSystem.Stop();
        destroyParticleSystem.transform.position = transform.position;
        destroyParticleSystem.Play();
        _gameManager.DestroyPlayerCallback();
        gameObject.SetActive(false);
    }

    internal void SetManager(GameManager gameManager)
    {
        _gameManager = gameManager;
    }

    internal void SetPause()
    {
        _pauseVelocity = _rb.velocity;
        _rb.velocity = Vector2.zero;
        thrusterParticleSystem.Pause();
    }

    internal void SetResume()
    {
        _rb.velocity = _pauseVelocity;
        thrusterParticleSystem.Play();
    }

    public void Restart()
    {
        GameObject o;
        (o = gameObject).SetActive(true);
        o.transform.position = startPosition;
        thrusterParticleSystem.Play();
    }
}