/*
MIT License
Copyright (c) 2021 Anton Kesy
Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:
The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.
THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
 */

using System.Collections.Generic;
using UnityEngine;

/**
 * Level Manager
 *
 * Generates, moves and deletes Chunks
 */
public class LevelManager : MonoBehaviour
{
    [Header("Chunk & Chunk Groups")] [SerializeField] [Range(0, 1f)]
    private float maxRandomOffset;

    [SerializeField] private float coinOffsetToBar = .25f;
    [SerializeField] private Transform chunkGroupTransform0;
    [SerializeField] private Transform chunkGroupTransform1;
    [SerializeField] private GameObject chunkPrefab;
    private float _chunkHeight;
    private float _halfChunkWidth;
    private float _chunkSpeed;
    private float _chunkYStart;
    private int _amountOfChunksToBuffer;
    [SerializeField] private float yOffsetToChunks;
    [SerializeField] private float xOffset;

    private readonly List<KeyValuePair<Transform, ChunkManager>> _chunks =
        new List<KeyValuePair<Transform, ChunkManager>>();

    private bool _isRight;
    private float _fistChunkYPosition;

    [Header("Level Properties")] [SerializeField]
    private float speedAdder;

    [SerializeField] private float chunkSpeedBase;

    [Header("Walls")] [SerializeField] private GameObject[] wallGameObjects;

    [SerializeField] private float xOffsetWall;

    private bool _isFirstChunkGroupBottom = true;

    [SerializeField] private float chunkDespawnTime = 4f;

    private void Start()
    {
        SetChunkVars();
        SetsWalls();
    }

    /**
     * Sets the chunk variables (height, amount, start position,...) according to mainCam
     */
    private void SetChunkVars()
    {
        //Chunk Height
        _chunkHeight = chunkPrefab.transform.localScale.y;
        //SetChunkStart
        var mainCam = Camera.main;
        if (mainCam is { })
        {
            var frustumHeight = 2.0f * mainCam.orthographicSize *
                                Mathf.Tan(mainCam.fieldOfView * 0.5f * Mathf.Deg2Rad);
            _chunkYStart = frustumHeight + _chunkHeight;
        }
        else
        {
            //Backup
            _chunkYStart = _amountOfChunksToBuffer * (_chunkHeight + yOffsetToChunks) / 2f;
        }

        //amountOfChunks
        _amountOfChunksToBuffer = (int) (_chunkYStart * 2 / (_chunkHeight + yOffsetToChunks) * .65f) + 1;
        //maxRandomOffset
        var localScale = chunkPrefab.transform.localScale;
        maxRandomOffset = (localScale.x - xOffset) * maxRandomOffset;

        _halfChunkWidth = localScale.x / 2f;
    }

    /**
     * Sets the walls according to the xOffsetWall
     */
    private void SetsWalls()
    {
        wallGameObjects[0].transform.position = new Vector3(xOffsetWall, 0, 0);
        wallGameObjects[1].transform.position = new Vector3(-xOffsetWall, 0, 0);
    }

    /**
     * Resets Level to Start -> Resets Chunks & Speed
     */
    internal void StartFreshLevel()
    {
        _isRight = Random.Range(0f, 1f) > 0.5f;
        _chunkSpeed = chunkSpeedBase;
        SetChunks();
    }

    /**
     * Sets all Chunks to start position
     */
    private void SetChunks()
    {
        var yOffset = 0f;
        //LowerChunkGroup && UpperChunkGroup
        for (var j = 0; j < 2; ++j)
        {
            for (var i = 0; i < _amountOfChunksToBuffer; ++i)
            {
                GenerateChunk(yOffset);
                yOffset += yOffsetToChunks + _chunkHeight;
            }

            _isFirstChunkGroupBottom = false;
        }

        //SetFirstIsBottom
        _isFirstChunkGroupBottom = true;

        //Set isRight after Generation
        if (_amountOfChunksToBuffer % 2 != 0)
        {
            _isRight = !_isRight;
        }
    }

    // ReSharper disable Unity.PerformanceAnalysis
    /**
     * Generates Chunk and Sets it to position in ChunkGroup
     */
    private void GenerateChunk(float yOffset)
    {
        //TODO("reuse chunks after first spawn")
        var chunk = Instantiate(chunkPrefab, _isFirstChunkGroupBottom ? chunkGroupTransform0 : chunkGroupTransform1);
        var chunkManager = chunk.GetComponent<ChunkManager>();
        _chunks.Add(new KeyValuePair<Transform, ChunkManager>(chunk.transform, chunkManager));
        ChangeChunk(chunk.transform, chunkManager, yOffset);
    }

    /**
     * Sets Position of Chunk, Lets Chunk Spawn Coin
     */
    private void ChangeChunk(Transform chunk, ChunkManager chunkManager, float yOffset)
    {
        chunk.position = GetNewChunkPosition(yOffset);
        chunkManager.SpawnCoin(GetNewCoinPosition(chunk.transform.position), _isRight);
        _isRight = !_isRight;
    }

    /**
     * Return new position of coin in Chunk
     */
    private Vector3 GetNewCoinPosition(Vector3 parentChunkPosition)
    {
        return parentChunkPosition + Vector3.right *
            (_isRight ? -coinOffsetToBar - _halfChunkWidth : coinOffsetToBar + _halfChunkWidth);
    }

    /**
     * Return new position of chunk
     */
    private Vector3 GetNewChunkPosition(float yOffset)
    {
        return new Vector3((Random.Range(0, maxRandomOffset) + xOffset) * (_isRight ? 1 : -1), _chunkYStart + yOffset,
            0);
    }

    private void Update()
    {
        MoveChunkGroups();
    }

    /**
     * Moves Chunk Groups & firstChunkYPosition according to speed and checks if chunk needs reset 
     */
    private void MoveChunkGroups()
    {
        chunkGroupTransform0.position += Vector3.down * (_chunkSpeed * Time.deltaTime);
        chunkGroupTransform1.position += Vector3.down * (_chunkSpeed * Time.deltaTime);

        _fistChunkYPosition += _chunkSpeed * Time.deltaTime;
        CheckChunkReset();
    }

    /**
     * Checks if Chunk needs reset and calls reset
     */
    private void CheckChunkReset()
    {
        if (_fistChunkYPosition > (_chunkHeight + yOffsetToChunks) * _amountOfChunksToBuffer)
        {
            ResetChunks();
        }
    }

    /**
     * Resets ChunkGroup
     */
    private void ResetChunks()
    {
        int start, stop;
        if (_isFirstChunkGroupBottom)
        {
            chunkGroupTransform1.position = Vector3.zero;
            start = _amountOfChunksToBuffer;
            stop = _chunks.Count;
        }
        else
        {
            chunkGroupTransform0.position = Vector3.zero;
            start = 0;
            stop = _amountOfChunksToBuffer;
        }

        var yOffset = 0f;
        //Resets Position of Chunk in ChunkGroup
        for (var i = start; i < stop; ++i)
        {
            ChangeChunk(_chunks[i].Key, _chunks[i].Value, yOffset);
            yOffset += yOffsetToChunks + _chunkHeight;
        }

        _isFirstChunkGroupBottom = !_isFirstChunkGroupBottom;
        _fistChunkYPosition = 0; //not called in GenerateChunk for easyRead
    }


    /**
     * Adds speedAdder to chunkSpeed
     */
    internal void AddSpeed()
    {
        _chunkSpeed += speedAdder;
    }


    /**
     * Stops all chunk movement
     */
    private void StopChunks()
    {
        _chunkSpeed = 0;
    }

    /**
     * Restarts Level
     */
    internal void RestartLevel()
    {
        ResetLevel();
        StartFreshLevel();
    }

    /**
     * Ends Level
     *
     * Stops movement and calls chunks to move out
     */
    internal void EndLevel()
    {
        StopChunks();
        foreach (var keyValuePair in _chunks)
        {
            keyValuePair.Value.MoveOutCall(chunkDespawnTime);
        }
    }

    /**
     * Resets Level completely
     */
    private void ResetLevel()
    {
        chunkGroupTransform0.position = Vector3.zero;
        chunkGroupTransform1.position = Vector3.zero;
        foreach (var keyValuePair in _chunks)
        {
            Destroy(keyValuePair.Key.gameObject);
        }

        _chunks.Clear();
        _fistChunkYPosition = 0;
        _isFirstChunkGroupBottom = true;
    }
}