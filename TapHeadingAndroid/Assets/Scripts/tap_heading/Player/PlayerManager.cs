using System.Collections;
using tap_heading.Game.level.obstacle;
using tap_heading.manager;
using UnityEngine;

namespace tap_heading.Player
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

        /**
     * Sets spawn y position of player depending on Camera
     */
        private void SetSpawnStartPositionY()
        {
            var mainCam = UnityEngine.Camera.main;
            if (mainCam is null) return;
            var frustumHeight = 2.0f * mainCam.orthographicSize *
                                Mathf.Tan(mainCam.fieldOfView * 0.5f * Mathf.Deg2Rad);
            _spawnStartPositionY = frustumHeight * -1f;
        }

        /**
     * Starts player moving in random direction
     */
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

        /**
     * Returns if player direction is changeable
     * changes player direction
     */
        public bool ChangeDirection()
        {
            if (!_isDirectionChangeable) return false;
            _direction = _direction == IPlayerManager.Direction.Left
                ? IPlayerManager.Direction.Right
                : IPlayerManager.Direction.Left;
            _rb.velocity = Vector2.zero;
            _rb.AddForce(
                _direction == IPlayerManager.Direction.Right ? Vector2.right * sideSpeed : Vector2.left * sideSpeed,
                ForceMode2D.Impulse);
            ChangeShadowPosition();
            return true;
        }

        /**
     * Sets player shadow position according to direction is heading to
     */
        private void ChangeShadowPosition()
        {
            shadowTransform.localPosition = Vector3.right *
                                            (_direction == IPlayerManager.Direction.Right
                                                ? -shadowOffset
                                                : shadowOffset) +
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
            var coin = coinGameObject.GetComponent<Coin>();
            coin.PickUp();
            managers.GetGameManager().CoinPickedUpCallback();
        }

        /**
    * Destroys Player
    */
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