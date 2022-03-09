﻿using System.Collections.Generic;
using UnityEngine;

namespace TapHeading.Game.Level.Obstacle.Manager
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

        private readonly List<IObstacle> _obstacles = new List<IObstacle>();

        private IObstacle.Side _nextSide;

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
            var chunkHeight = obstaclePrefab.transform.localScale.y;
            var mainCam = UnityEngine.Camera.main;
            if (mainCam is { })
            {
                var frustumHeight = 2.0f * mainCam.orthographicSize *
                                    Mathf.Tan(mainCam.fieldOfView * 0.5f * Mathf.Deg2Rad);
                _yStartHeight = frustumHeight + chunkHeight / 2f;
                _minSightHeight = -(frustumHeight + chunkHeight / 2f);
                _amountOfObstaclesBuffer = (int) (frustumHeight * 2 / yOffsetBetweenObstacles);
            }
            else
            {
                Application.Quit(1);
            }

            maxRandomOffset = (obstaclePrefab.transform.localScale.x - xOffset) * maxRandomOffset;

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
                ResetObstacle(obstacle, yOffset);
                yOffset += yOffsetBetweenObstacles + obstaclePrefab.transform.localScale.y;
            }
        }

        private void ResetObstacle(IObstacle obstacle, float yOffset)
        {
            obstacle.Reset(GetNewChunkPosition(yOffset), _nextSide);
            SwitchSide();
        }

        private void GenerateObstacle()
        {
            var obstacle = Instantiate(obstaclePrefab, transform);
            obstacle.transform.position = Vector3.up * _yStartHeight;
            var obstacleManager = obstacle.GetComponent<IObstacle>();
            _obstacles.Add(obstacleManager);
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
                obstacle.Move(downVector);
                if (obstacle.GetYPos() <= _minSightHeight)
                {
                    ResetObstacle(obstacle, 0f);
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
                keyValuePair.DeSpawn();
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