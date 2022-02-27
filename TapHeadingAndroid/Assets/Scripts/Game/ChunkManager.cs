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

using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

/**
 * Manages Coin, Despawn-Movement of Single Chunk
 */
public class ChunkManager : MonoBehaviour
{
    [Header("Coin")] [SerializeField] private GameObject coinGameObject;
    [SerializeField] private float coinSpawnProbability;

    private bool _isRight;


    /**
     * Sets the coin by probability to spawn or deactivates it
     */
    internal void SpawnCoin(Vector3 position, bool isRight)
    {
        _isRight = isRight;
        if (coinSpawnProbability > Random.Range(0, 1f))
        {
            coinGameObject.transform.position = position;
            coinGameObject.SetActive(true);
        }
        else
        {
            coinGameObject.SetActive(false);
        }
    }

    /**
     * Hides Coin
     */
    private void DestroyCall()
    {
        coinGameObject.SetActive(false);
    }

    /**
     * Starts Chunk MoveOut
     */
    internal void MoveOutCall(float duration)
    {
        DestroyCall();
        StartCoroutine(MoveOut(duration));
    }

    /**
     * Moves out this Chunk
     */
    private IEnumerator MoveOut(float duration)
    {
        var time = 0f;
        var position = transform.position;
        var targetPosition = new Vector3((_isRight ? 15f : -15f), position.y, position.z);
        while (time < duration)
        {
            transform.position = Vector3.LerpUnclamped(transform.position, targetPosition, time / duration);
            time += Time.deltaTime;
            yield return null;
            //early stop of Lerp
            if (!(Mathf.Abs(targetPosition.x) - Mathf.Abs(transform.position.x) < 1)) continue;
            transform.position = targetPosition;
            break;
        }
    }
}