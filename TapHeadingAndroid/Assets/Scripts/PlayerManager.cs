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

    private Vector2 _pauseVelocity;

    private float _spawnStartPositionY = 20;

    [SerializeField] private float spawnTime;

    private Coroutine _spawnCoroutine = null;

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
        if (_spawnCoroutine != null)
        {
            StopCoroutine(_spawnCoroutine);
            transform.position = Vector3.up * startPosition;
            _spawnCoroutine = null;
        }

        ChangeDirection();
        if (Random.Range(0f, 1f) > .5f)
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
        ChangeShadowPosition();
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
        StartMoving();
    }

    public void ResetPlayer()
    {
        GameObject o;
        (o = gameObject).SetActive(true);
        o.transform.position = startPosition;
    }

    internal void SpawnPlayer()
    {
        _spawnCoroutine = StartCoroutine(MovePlayerToSpawn());
    }

    private IEnumerator MovePlayerToSpawn()
    {
        float time = 0;
        var targetPosition = Vector3.up * startPosition;
        transform.position = Vector3.up * _spawnStartPositionY;
        thrusterParticleSystem.Play();

        while (time < spawnTime)
        {
            transform.position = Vector3.Lerp(transform.position, targetPosition, time / spawnTime);
            time += Time.deltaTime;
            if (Vector3.Distance(transform.position, targetPosition) <= 0.5f)
            {
                thrusterParticleSystem.Stop();
            }

            yield return null;
        }

        transform.position = targetPosition;

        yield return null;
    }
}