using System.Collections.Generic;
using TapHeading.Camera.Utility;
using UnityEngine;

namespace TapHeading.Game.Level.Obstacle.Manager
{
    public class ObstacleManager : MonoBehaviour, IObstacleManager
    {
        [SerializeField] [Range(0, 1f)] private float maxRandomOffset;
        [SerializeField] private GameObject obstaclePrefab;
        [SerializeField] private float yOffsetBetweenObstacles = 6f;
        [SerializeField] private float xOffset = 5.5f;
        private readonly List<IObstacle> _obstacles = new List<IObstacle>();
        private IObstacle.Side _nextSide;
        private float _speed, _yStartHeight, _yEndHeight;
        private int _obstaclesBuffered;

        private void Start()
        {
            Setup();
        }

        private void Update()
        {
            MoveObstacles();
        }

        private void Setup()
        {
            var obstacleLocalScale = obstaclePrefab.transform.localScale;
            var chunkHeight = obstacleLocalScale.y;
            var frustumHeight = CameraUtility.GetFrustumHeight();
            _yStartHeight = frustumHeight + chunkHeight / 2f;
            _yEndHeight = -(frustumHeight + chunkHeight / 2f);
            _obstaclesBuffered = (int) (frustumHeight * 2 / yOffsetBetweenObstacles);

            maxRandomOffset = (obstacleLocalScale.x - xOffset) * maxRandomOffset;

            for (var i = 0; i < _obstaclesBuffered; ++i)
            {
                GenerateObstacle();
            }
        }

        private void ResetPositions()
        {
            var yOffset = 0f;
            foreach (var obstacle in _obstacles)
            {
                ResetObstacle(obstacle, yOffset);
                yOffset += yOffsetBetweenObstacles + obstaclePrefab.transform.localScale.y;
            }
        }

        private void ResetObstacle(IObstacle obstacle, float yOffset)
        {
            obstacle.ResetTo(GetNewObstaclePosition(yOffset), _nextSide);
            SwitchSide();
        }

        private void GenerateObstacle()
        {
            var obstacle = Instantiate(obstaclePrefab, transform);
            obstacle.transform.position = Vector3.up * _yStartHeight;
            var obstacleManager = obstacle.GetComponent<IObstacle>();
            _obstacles.Add(obstacleManager);
        }


        private Vector3 GetNewObstaclePosition(float yOffset)
        {
            var randomXInRange = Random.Range(0, maxRandomOffset) + xOffset;
            randomXInRange *= _nextSide == IObstacle.Side.Right ? 1 : -1;
            return new Vector3(randomXInRange, _yStartHeight + yOffset, 0);
        }


        private void MoveObstacles()
        {
            var downVector = Vector3.down * (_speed * Time.deltaTime);
            foreach (var obstacle in _obstacles)
            {
                obstacle.Move(downVector);
                if (obstacle.GetYPos() <= _yEndHeight)
                {
                    ResetObstacle(obstacle, 0f);
                }
            }
        }

        public void SetSpeed(float speed)
        {
            _speed = speed;
        }

        public void Restart()
        {
            ResetPositions();
        }

        public void Stop()
        {
            _speed = 0f;
            foreach (var keyValuePair in _obstacles)
            {
                keyValuePair.DeSpawn();
            }
        }

        private void SwitchSide()
        {
            _nextSide = _nextSide == IObstacle.Side.Left ? IObstacle.Side.Right : IObstacle.Side.Left;
        }
    }
}