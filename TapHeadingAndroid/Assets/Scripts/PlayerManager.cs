using System;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    private GameManager _gameManager;

    [SerializeField] private float sideSpeed;

    private Rigidbody2D _rb;

    private bool _isDirectionRight;

    private Vector2 _pauseVelocity;

    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
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
        if (og.CompareTag("Wall"))
        {
            OnWallCollision();
        }
        else if (og.CompareTag("Bar"))
        {
            OnBarCollision();
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
        Destroy(coinGameObject);
        _gameManager.CoinPickedUpCallback();
    }

    private void OnWallCollision()
    {
        _gameManager.WallCollisionCallback();
    }

    private void OnBarCollision()
    {
        _gameManager.BarCollisionCallback();
    }

    internal void SetManager(GameManager gameManager)
    {
        _gameManager = gameManager;
    }

    internal void SetPause()
    {
        _pauseVelocity = _rb.velocity;
        _rb.velocity = Vector2.zero;
    }

    internal void SetResume()
    {
        _rb.velocity = _pauseVelocity;
    }
}