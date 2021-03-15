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

public class LevelManager : MonoBehaviour
{
    [SerializeField] [Range(0, 1f)] private float maxRandomOffset;
    [SerializeField] private float coinOffsetToBar = .25f;
    [SerializeField] private Transform chunkGroupTransform0;
    [SerializeField] private Transform chunkGroupTransform1;

    [SerializeField] private float speedAdder;

    [SerializeField] private GameObject chunkPrefab;
    private float _chunkHeight;
    private float _halfChunkWidth;
    [SerializeField] private float chunkSpeedBase;
    private float _chunkSpeed;
    private float _chunkYStart;
    private int _amountOfChunksToBuffer;

    [SerializeField] private float yOffsetToChunks;

    [SerializeField] private GameObject wallPrefab;

    [SerializeField] private float xOffsetWall;
    [SerializeField] private float xOffset;

    private readonly List<KeyValuePair<Transform, ChunkManager>> _chunks =
        new List<KeyValuePair<Transform, ChunkManager>>();

    private bool _isRight;

    private float _fistChunkYPosition;

    private bool _isPause;

    private float _pauseChunkSpeed;

    private bool _isFirstChunkGroupBottom = true;

    [SerializeField] private float chunkDespawnTime = 4f;

    private void Start()
    {
        _chunkHeight = chunkPrefab.transform.localScale.y;
        var mainCam = Camera.main;
        if (mainCam is { })
        {
            var frustumHeight = 2.0f * mainCam.orthographicSize *
                                Mathf.Tan(mainCam.fieldOfView * 0.5f * Mathf.Deg2Rad);
            _chunkYStart = frustumHeight + _chunkHeight + yOffsetToChunks;
        }
        else
        {
            //Backup
            _chunkYStart = _amountOfChunksToBuffer * (_chunkHeight + yOffsetToChunks) / 2f;
        }

        _amountOfChunksToBuffer = (int) (_chunkYStart * 2 / (_chunkHeight + yOffsetToChunks) * .65f) + 1;

        maxRandomOffset = (chunkPrefab.transform.localScale.x - xOffset) * maxRandomOffset;

        _halfChunkWidth = chunkPrefab.transform.localScale.x / 2f;

        GenerateWalls();
    }

    private void GenerateWalls()
    {
        Instantiate(wallPrefab, new Vector3(xOffsetWall, 0, 0), Quaternion.identity);
        Instantiate(wallPrefab, new Vector3(-xOffsetWall, 0, 0), Quaternion.identity);
    }

    internal void StartGame()
    {
        _isRight = Random.Range(0f, 1f) > 0.5f;
        _chunkSpeed = chunkSpeedBase;
        GenerateChunks();
    }

    private void GenerateChunks()
    {
        var yOffset = 0f;
        for (var i = 0; i < _amountOfChunksToBuffer; ++i)
        {
            GenerateChunk(yOffset);
            yOffset += yOffsetToChunks + _chunkHeight;
        }

        _isFirstChunkGroupBottom = false;
        for (var i = 0; i < _amountOfChunksToBuffer; ++i)
        {
            GenerateChunk(yOffset);
            yOffset += yOffsetToChunks + _chunkHeight;
        }

        if (_amountOfChunksToBuffer % 2 != 0)
        {
            _isRight = !_isRight;
        }

        _isFirstChunkGroupBottom = true;
    }

    // ReSharper disable Unity.PerformanceAnalysis
    private void GenerateChunk(float yOffset)
    {
        var chunk = Instantiate(chunkPrefab, _isFirstChunkGroupBottom ? chunkGroupTransform0 : chunkGroupTransform1);
        chunk.transform.position = GetNewChunkPosition(yOffset);


        var chunkManager = chunk.GetComponent<ChunkManager>();
        _chunks.Add(new KeyValuePair<Transform, ChunkManager>(chunk.transform, chunkManager));

        var coinPosition = chunk.transform.position + Vector3.right *
            (_isRight
                ? -coinOffsetToBar - _halfChunkWidth
                : coinOffsetToBar + _halfChunkWidth);

        chunkManager.SpawnCoin(coinPosition, _isRight);
        //prepare Next
        _isRight = !_isRight;
    }

    private Vector3 GetNewChunkPosition(float yOffset)
    {
        return new Vector3((Random.Range(0, maxRandomOffset) + xOffset) * (_isRight ? 1 : -1), _chunkYStart + yOffset,
            0);
    }

    // Update is called once per frame
    private void Update()
    {
        chunkGroupTransform0.position += Vector3.down * (_chunkSpeed * Time.deltaTime);
        chunkGroupTransform1.position += Vector3.down * (_chunkSpeed * Time.deltaTime);

        if (!_isPause)
        {
            _fistChunkYPosition += _chunkSpeed * Time.deltaTime;
            if (_fistChunkYPosition > (_chunkHeight + yOffsetToChunks) * _amountOfChunksToBuffer)
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
                    var newChunkPosition = GetNewChunkPosition(yOffset);
                    _chunks[i].Key.position = newChunkPosition;

                    var coinPosition = newChunkPosition + Vector3.right *
                        (_isRight
                            ? -coinOffsetToBar - _halfChunkWidth
                            : coinOffsetToBar + _halfChunkWidth);

                    _chunks[i].Value.SpawnCoin(coinPosition, _isRight);
                    //prepare for next
                    _isRight = !_isRight;
                    yOffset += yOffsetToChunks + _chunkHeight;
                }

                _isFirstChunkGroupBottom = !_isFirstChunkGroupBottom;
                _fistChunkYPosition = 0; //not called in GenerateChunk for easyRead
            }
        }
    }

    internal void AddSpeed()
    {
        _chunkSpeed += speedAdder;
    }

    public void Pause()
    {
        _isPause = true;
        PauseChunks();
    }

    private void PauseChunks()
    {
        _pauseChunkSpeed = _chunkSpeed;
        _chunkSpeed = 0;
    }

    public void Resume()
    {
        _isPause = false;
        ResumeChunks();
    }

    private void ResumeChunks()
    {
        _chunkSpeed = _pauseChunkSpeed;
    }

    internal void RestartGame()
    {
        ResetGame();
        _isPause = false;
        StartGame();
    }

    internal void LostGame()
    {
        foreach (var keyValuePair in _chunks)
        {
            keyValuePair.Value.MoveOutCall(chunkDespawnTime);
        }
    }


    private void ResetGame()
    {
        //TODO("Same side generate sometimes -> probably fixed -> nope" -> also on fresh start)
        chunkGroupTransform0.position = Vector3.zero;
        chunkGroupTransform1.position = Vector3.zero;
        foreach (var keyValuePair in _chunks)
        {
            keyValuePair.Value.DestroyCall();
            Destroy(keyValuePair.Key.gameObject);
        }

        _chunks.Clear();
        _fistChunkYPosition = 0;
        _isFirstChunkGroupBottom = true;
    }
}