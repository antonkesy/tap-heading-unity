using System.Collections;
using TapHeading.Camera.Utility;
using TapHeading.Game.Level.Obstacle;
using TapHeading.Manager;
using UnityEngine;

namespace TapHeading.Player
{
    public class PlayerManager : MonoBehaviour, IPlayerManager
    {
        [SerializeField] private ManagerCollector managers;
        [SerializeField] private ParticleSystem thrusterParticleSystem;
        [SerializeField] private ParticleSystem destroyParticleSystem;
        [SerializeField] private Transform shadowTransform;
        [SerializeField] private float shadowOffset;

        [Header("Properties")] [SerializeField]
        private float sideSpeed;

        [SerializeField] private float spawnTime;
        private Vector2 _startPosition;
        private Rigidbody2D _rb;
        private float _spawnStartPositionY = -20;
        private bool _isDirectionChangeable;
        private bool _isWaitingForRespawn;
        private bool _isRespawning;

        private IPlayerManager.Direction _direction = IPlayerManager.Direction.Left;

        private void Awake()
        {
            _startPosition = transform.position;
            _rb = GetComponent<Rigidbody2D>();
        }

        private void Start()
        {
            SetSpawnStartPositionY();
        }

        private void SetSpawnStartPositionY()
        {
            _spawnStartPositionY = CameraUtility.GetFrustumHeight() * -1f;
        }

        public void StartMoving()
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

        public bool ChangeDirection(IPlayerManager.Direction direction)
        {
            return direction == _direction && ChangeDirection();
        }

        public bool ChangeDirection()
        {
            if (!_isDirectionChangeable) return false;
            SwitchDirection();
            _rb.velocity = Vector2.zero;
            _rb.AddForce((_direction == IPlayerManager.Direction.Right ? Vector2.right : Vector2.left) * sideSpeed,
                ForceMode2D.Impulse);
            ChangeShadowPosition();
            return true;
        }

        private void SwitchDirection()
        {
            _direction = _direction == IPlayerManager.Direction.Left
                ? IPlayerManager.Direction.Right
                : IPlayerManager.Direction.Left;
        }

        private void ChangeShadowPosition()
        {
            var xVector = (_direction == IPlayerManager.Direction.Right ? Vector3.right : Vector3.left) * shadowOffset;
            var yVector = Vector3.down * shadowOffset;
            shadowTransform.localPosition = xVector + yVector;
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
            var coin = coinGameObject.GetComponent<Coin>();
            coin.PickUp();
            managers.GetGameManager().CoinPickedUpCallback();
        }

        private void DestroyPlayer()
        {
            _rb.velocity = Vector2.zero;
            thrusterParticleSystem.Stop();
            Instantiate(destroyParticleSystem, transform.position, Quaternion.identity);
            gameObject.SetActive(false);
            managers.GetGameManager().DestroyPlayerCallback();
        }

        public void Spawn()
        {
            gameObject.SetActive(true);
            transform.position = Vector3.up * _spawnStartPositionY;
            StartCoroutine(MovePlayerToSpawn());
        }

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
}