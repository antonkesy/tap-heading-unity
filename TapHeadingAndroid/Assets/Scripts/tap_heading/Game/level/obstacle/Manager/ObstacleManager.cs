using System.Collections.Generic;
using UnityEngine;

namespace tap_heading.Game.level.obstacle.Manager
{
    public class ObstacleManager : MonoBehaviour, IObstacleManager
    {
        [SerializeField] [Range(0, 1f)] private float maxRandomOffset;

        [SerializeField] private GameObject obstaclePrefab;

        private float _chunkSpeed;
        private float _yStartHeight;
        private float _minSightHeight;

        private int _amountOfObstaclesBuffer;
        [SerializeField] private float yOffsetBetweenObstacles = 6f;
        [SerializeField] private float xOffset = 5.5f;

        private readonly List<KeyValuePair<Transform, IObstacle>> _obstacles =
            new List<KeyValuePair<Transform, IObstacle>>();

        private IObstacle.Side _nextSide;

        private void Start()
        {
            SetChunkVars();
            BuildChunks();
        }

        private void Update()
        {
            MoveObstacles();
        }

        private void SetChunkVars()
        {
            var chunkHeight = obstaclePrefab.transform.localScale.y;
            var mainCam = UnityEngine.Camera.main;
            if (mainCam is { })
            {
                var frustumHeight = 2.0f * mainCam.orthographicSize *
                                    Mathf.Tan(mainCam.fieldOfView * 0.5f * Mathf.Deg2Rad);
                _yStartHeight = frustumHeight + chunkHeight / 2f;
                _minSightHeight = -(frustumHeight + chunkHeight / 2f);
            }

            _amountOfObstaclesBuffer = (int) (_yStartHeight * 2 / (chunkHeight + yOffsetBetweenObstacles) * .65f) + 1;
            maxRandomOffset = (obstaclePrefab.transform.localScale.x - xOffset) * maxRandomOffset;
        }

        private void BuildChunks()
        {
            for (var i = 0; i < _amountOfObstaclesBuffer; ++i)
            {
                GenerateObstacle();
            }
        }

        private void ResetPositions()
        {
            var yOffset = 0f;
            foreach (var obstacle in _obstacles)
            {
                obstacle.Key.transform.position = GetNewChunkPosition(yOffset);
                yOffset += yOffsetBetweenObstacles + obstaclePrefab.transform.localScale.y;
            }
        }

        private void GenerateObstacle()
        {
            var obstacle = Instantiate(obstaclePrefab, transform);
            obstacle.transform.position = Vector3.up * _yStartHeight;
            var obstacleManager = obstacle.GetComponent<IObstacle>();
            _obstacles.Add(new KeyValuePair<Transform, IObstacle>(obstacle.transform, obstacleManager));
        }


        private Vector3 GetNewChunkPosition(float yOffset)
        {
            return new Vector3(
                (Random.Range(0, maxRandomOffset) + xOffset) * (_nextSide == IObstacle.Side.Right ? 1 : -1),
                _yStartHeight + yOffset,
                0);
        }


        private void MoveObstacles()
        {
            var downVector = Vector3.down * (_chunkSpeed * Time.deltaTime);
            foreach (var obstacle in _obstacles)
            {
                obstacle.Key.position += downVector;
                if (obstacle.Key.position.y <= _minSightHeight + obstacle.Key.transform.localScale.y)
                {
                    obstacle.Value.Reset(GetNewChunkPosition(0f), _nextSide);
                    SwitchSide();
                }
            }
        }

        public void SetSpeed(float speed)
        {
            _chunkSpeed = speed;
        }

        public void Restart()
        {
            ResetPositions();
        }

        public void Stop()
        {
            _chunkSpeed = 0f;
            foreach (var keyValuePair in _obstacles)
            {
                keyValuePair.Value.DeSpawn();
            }
        }

        private void SwitchSide()
        {
            _nextSide = _nextSide == IObstacle.Side.Left
                ? IObstacle.Side.Right
                : IObstacle.Side.Left;
        }
    }
}