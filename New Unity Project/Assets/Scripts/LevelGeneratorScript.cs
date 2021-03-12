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
    [SerializeField] private int amountOfChunksToBuffer = 10;

    [SerializeField] private float yOffsetToChunks;

    [SerializeField] private GameObject wallPrefab;
    [SerializeField] private float xOffset;

    private readonly List<ChunkManager> _chunks = new List<ChunkManager>();

    private bool _isRight;

    private float _lastChunkPosition;

    private void Start()
    {
        //Todo("set DestroyPosition")
        //Todo("set xOffset")
        _chunkSize = chunkPrefab.transform.localScale.y;
        _chunkYStart = amountOfChunksToBuffer * (_chunkSize + yOffsetToChunks) / 2f;
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
        var chunk = Instantiate(chunkPrefab, new Vector3(0, _chunkYStart, 0), Quaternion.identity);
        var chunkManager = chunk.GetComponent<ChunkManager>();
        chunkManager.SetUp(xOffset, _isRight, _chunkSpeed);
        _chunks.Add(chunkManager);
        Debug.Log("Add" + _chunks.Count);
        _isRight = !_isRight;
    }

    // Update is called once per frame
    private void Update()
    {
        _lastChunkPosition += _chunkSpeed * Time.deltaTime;
        if (_lastChunkPosition > _chunkSize + yOffsetToChunks)
        {
            if (_chunks.Count > amountOfChunksToBuffer)
            {
                /*
                 * if (gameObject.transform.position.y <= _destroyPosition)
            {
            Destroy(gameObject);
            }
                 */
                _chunks[0].DestroyCall();
                _chunks.RemoveAt(0);
            }

            _lastChunkPosition = 0; //not called in GenerateChunk for easyRead
            GenerateChunk();
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
}