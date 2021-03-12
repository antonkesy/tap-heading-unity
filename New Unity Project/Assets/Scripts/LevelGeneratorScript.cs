using UnityEngine;

public class LevelGeneratorScript : MonoBehaviour
{
    [SerializeField] private float speedAdder;

    [SerializeField] private GameObject chunkPrefab;
    private float _chunkSize;
    [SerializeField] private float chunkSpeedBase;
    private float _chunkSpeed;
    [SerializeField] private float chunkYStart;

    [SerializeField] private float yOffsetToChunks;

    [SerializeField] private GameObject wallPrefab;
    [SerializeField] private float xOffset;
    [SerializeField] private float destroyPosition;

    private bool _isRight;

    private float _lastChunkPosition;

    private void Start()
    {
        //Todo("set DestroyPosition")
        //Todo("set xOffset")
        _chunkSize = chunkPrefab.transform.localScale.y;
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
        var chunk = Instantiate(chunkPrefab, new Vector3(0, chunkYStart, 0), Quaternion.identity);
        chunk.GetComponent<ChunkManager>().SetUp(xOffset, _isRight, _chunkSpeed, destroyPosition);
        _isRight = !_isRight;
    }

    // Update is called once per frame
    private void Update()
    {
        _lastChunkPosition += _chunkSpeed * Time.deltaTime;
        if (_lastChunkPosition > _chunkSize + yOffsetToChunks)
        {
            _lastChunkPosition = 0; //not called in GenerateChunk for easyRead
            GenerateChunk();
        }
    }

    internal void AddSpeed()
    {
        _chunkSpeed += speedAdder;
    }
}