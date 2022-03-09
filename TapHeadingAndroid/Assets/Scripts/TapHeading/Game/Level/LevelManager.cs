using TapHeading.Game.Level.Obstacle.Manager;
using UnityEngine;

namespace TapHeading.Game.Level
{
    public class LevelManager : MonoBehaviour, ILevelManager
    {
        [Header("Walls")] [SerializeField] private GameObject[] wallGameObjects;
        [SerializeField] private ObstacleManager obstacleManager;
        [SerializeField] private float xOffsetWall;

        [Header("Level Properties")] [SerializeField]
        private float speedIncreaseBy = .5f;

        [SerializeField] private float baseSpeed = 5f;

        private float _speed;

        private void Awake()
        {
            SetsWalls();
        }

        private void SetsWalls()
        {
            wallGameObjects[0].transform.position = new Vector3(xOffsetWall, 0, 0);
            wallGameObjects[1].transform.position = new Vector3(-xOffsetWall, 0, 0);
        }

        public void IncreaseSpeed()
        {
            _speed += speedIncreaseBy;
            obstacleManager.SetSpeed(_speed);
        }

        public void Restart()
        {
            obstacleManager.Restart();
            _speed = baseSpeed;
            obstacleManager.SetSpeed(_speed);
        }

        public void Stop()
        {
            obstacleManager.Stop();
        }
    }
}