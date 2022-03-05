using tap_heading.Game.level.chunk;
using UnityEngine;

namespace tap_heading.Game.level
{
    public class LevelManager : MonoBehaviour, ILevelManager
    {
        [Header("Walls")] [SerializeField] private GameObject[] wallGameObjects;
        [SerializeField] private ChunkManager chunkManager;
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
            chunkManager.SetSpeed(_speed);
        }

        public void Restart()
        {
            chunkManager.Restart();
            _speed = baseSpeed;
            chunkManager.SetSpeed(_speed);
        }

        public void Stop()
        {
            chunkManager.Stop();
        }
    }
}