using System.Collections.Generic;
using UnityEngine;

public class LevelGeneratorScript : MonoBehaviour
{
    [SerializeField] private float speedAdder;

    [SerializeField] private GameObject chunkPrefab;
    private float _chunkSize;
    [SerializeField] private float chunkSpeedBase;
    private float _chunkSpeed;
    private float _chunkYStart;
    private int _amountOfChunksToBuffer;

    [SerializeField] private float yOffsetToChunks;

    [SerializeField] private GameObject wallPrefab;
    [SerializeField] private float xOffset;

    private readonly List<ChunkManager> _chunks = new List<ChunkManager>();

    private bool _isRight;

    private float _lastChunkPosition;

    private bool _isPause;

    private void Start()
    {
        _chunkSize = chunkPrefab.transform.localScale.y;
        var mainCam = Camera.main;
        if (mainCam is { })
        {
            var frustumHeight = 2.0f * mainCam.orthographicSize *
                                Mathf.Tan(mainCam.fieldOfView * 0.5f * Mathf.Deg2Rad);
            _chunkYStart = frustumHeight + _chunkSize + yOffsetToChunks;
        }
        else
        {
            //Backup
            _chunkYStart = _amountOfChunksToBuffer * (_chunkSize + yOffsetToChunks) / 2f;
        }

        _amountOfChunksToBuffer = (int) (_chunkYStart * 2 / (_chunkSize + yOffsetToChunks)) + 1;

        GenerateWalls();
    }

    private void GenerateWalls()
    {
        Instantiate(wallPrefab, new Vector3(xOffset, 0, 0), Quaternion.identity);
        Instantiate(wallPrefab, new Vector3(-xOffset, 0, 0), Quaternion.identity);
    }

    internal void StartGame()
    {
        _chunkSpeed = chunkSpeedBase;
        GenerateChunk();
    }

    // ReSharper disable Unity.PerformanceAnalysis
    private void GenerateChunk()
    {
        var chunk = Instantiate(chunkPrefab, new Vector3(Random.Range(0, _isRight ? 3f : -3f), _chunkYStart, 0),
            Quaternion.identity);
        var chunkManager = chunk.GetComponent<ChunkManager>();
        chunkManager.SetUp(xOffset, _isRight, _chunkSpeed);
        _chunks.Add(chunkManager);
        _isRight = !_isRight;
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        if (!_isPause)
        {
            _lastChunkPosition += _chunkSpeed * Time.fixedDeltaTime;
            if (_lastChunkPosition > _chunkSize + yOffsetToChunks)
            {
                if (_chunks.Count > _amountOfChunksToBuffer)
                {
                    _chunks[0].DestroyCall();
                    _chunks.RemoveAt(0);
                }

                _lastChunkPosition = 0; //not called in GenerateChunk for easyRead
                GenerateChunk();
            }
        }
    }

    internal void AddSpeed()
    {
        _chunkSpeed += speedAdder;
        foreach (var chunk in _chunks)
        {
            chunk.UpdateSpeed(_chunkSpeed);
        }
    }

    public void Pause()
    {
        //_timePause = _lastChunkPosition;
        _isPause = true;
        PauseChunks();
    }

    private void PauseChunks()
    {
        foreach (var chunk in _chunks)
        {
            chunk.UpdateSpeed(0);
        }
    }

    public void Resume()
    {
        //_lastChunkPosition = _timePause;
        _isPause = false;
        ResumeChunks();
    }

    private void ResumeChunks()
    {
        foreach (var chunk in _chunks)
        {
            chunk.UpdateSpeed(_chunkSpeed);
        }
    }
}