using System.Collections.Generic;
using tap_heading.Game.level.obstacle;
using UnityEngine;

namespace tap_heading.Game.level.chunk
{
    public class ChunkManager : MonoBehaviour
    {
        [Header("Chunk & Chunk Groups")] [SerializeField] [Range(0, 1f)]
        private float maxRandomOffset;

        [SerializeField] private Transform chunkGroupTransform0;
        [SerializeField] private Transform chunkGroupTransform1;

        [SerializeField] private GameObject obstaclePrefab;

        private float _chunkHeight;

        private float _chunkSpeed;
        private float _chunkYStart;
        private int _amountOfObstaclesBuffer;
        [SerializeField] private float yOffsetToChunks;
        [SerializeField] private float xOffset;

        private readonly List<KeyValuePair<Transform, IObstacleManager>> _chunks =
            new List<KeyValuePair<Transform, IObstacleManager>>();

        private IObstacleManager.Side _side;
        private float _fistChunkYPosition;

        private bool _isFirstChunkGroupBottom = true;

        private void Start()
        {
            SetChunkVars();
        }

        private void Update()
        {
            MoveChunkGroups();
            CheckChunkReset();
        }

        private void SetChunkVars()
        {
            _chunkHeight = obstaclePrefab.transform.localScale.y;
            var mainCam = UnityEngine.Camera.main;
            if (mainCam is { })
            {
                var frustumHeight = 2.0f * mainCam.orthographicSize *
                                    Mathf.Tan(mainCam.fieldOfView * 0.5f * Mathf.Deg2Rad);
                _chunkYStart = frustumHeight + _chunkHeight;
            }
            else
            {
                _chunkYStart = _amountOfObstaclesBuffer * (_chunkHeight + yOffsetToChunks) / 2f;
            }

            _amountOfObstaclesBuffer = (int) (_chunkYStart * 2 / (_chunkHeight + yOffsetToChunks) * .65f) + 1;
            maxRandomOffset = (obstaclePrefab.transform.localScale.x - xOffset) * maxRandomOffset;
        }

        private void SetChunks()
        {
            var yOffset = 0f;
            //LowerChunkGroup && UpperChunkGroup
            for (var j = 0; j < 2; ++j)
            {
                for (var i = 0; i < _amountOfObstaclesBuffer; ++i)
                {
                    GenerateChunk(yOffset);
                    yOffset += yOffsetToChunks + _chunkHeight;
                }

                _isFirstChunkGroupBottom = false;
            }

            _isFirstChunkGroupBottom = true;

            if (_amountOfObstaclesBuffer % 2 != 0)
            {
                SwitchSide();
            }
        }


        private void GenerateChunk(float yOffset)
        {
            //TODO("reuse chunks after first spawn")
            var chunk = Instantiate(obstaclePrefab,
                _isFirstChunkGroupBottom ? chunkGroupTransform0 : chunkGroupTransform1);
            var chunkManager = chunk.GetComponent<IObstacleManager>();
            chunkManager.SetSide(_side);
            _chunks.Add(new KeyValuePair<Transform, IObstacleManager>(chunk.transform, chunkManager));
            SetPosition(chunk.transform, yOffset);
            SwitchSide();
        }

        private void SetPosition(Transform chunk, float yOffset)
        {
            chunk.position = GetNewChunkPosition(yOffset);
        }


        private Vector3 GetNewChunkPosition(float yOffset)
        {
            return new Vector3(
                (Random.Range(0, maxRandomOffset) + xOffset) * (_side == IObstacleManager.Side.Right ? 1 : -1),
                _chunkYStart + yOffset,
                0);
        }


        private void MoveChunkGroups()
        {
            var downVector = Vector3.down * (_chunkSpeed * Time.deltaTime);
            chunkGroupTransform0.position += downVector;
            chunkGroupTransform1.position += downVector;

            _fistChunkYPosition += downVector.y;
        }

        private void CheckChunkReset()
        {
            if (_fistChunkYPosition > (_chunkHeight + yOffsetToChunks) * _amountOfObstaclesBuffer)
            {
                ResetChunks();
            }
        }

        private void ResetChunks()
        {
            int start, stop;
            if (_isFirstChunkGroupBottom)
            {
                chunkGroupTransform1.position = Vector3.zero;
                start = _amountOfObstaclesBuffer;
                stop = _chunks.Count;
            }
            else
            {
                chunkGroupTransform0.position = Vector3.zero;
                start = 0;
                stop = _amountOfObstaclesBuffer;
            }

            var yOffset = 0f;
            //Resets Position of Chunk in ChunkGroup
            for (var i = start; i < stop; ++i)
            {
                SetPosition(_chunks[i].Key, yOffset);
                yOffset += yOffsetToChunks + _chunkHeight;
            }

            _isFirstChunkGroupBottom = !_isFirstChunkGroupBottom;
            _fistChunkYPosition = 0; //not called in GenerateChunk for easyRead
        }


        /**
              * Adds speedAdder to chunkSpeed
              */
        public void SetSpeed(float speed)
        {
            _chunkSpeed = speed;
        }

        public void Restart()
        {
            //TODO kill and rebuild new chunks
            ResetChunks();
            ResetLevel(); //TODO ? why two different?
        }

        public void Stop()
        {
            _chunkSpeed = 0f;
            foreach (var keyValuePair in _chunks)
            {
                keyValuePair.Value.DeSpawn();
            }
        }

        private void ResetLevel()
        {
            chunkGroupTransform0.position = Vector3.zero;
            chunkGroupTransform1.position = Vector3.zero;
            foreach (var keyValuePair in _chunks)
            {
                Destroy(keyValuePair.Key.gameObject);
            }

            _chunks.Clear();
        }

        private void SwitchSide()
        {
            _side = _side == IChunkManager.Side.Left ? IChunkManager.Side.Right : IChunkManager.Side.Left;
        }
    }
}