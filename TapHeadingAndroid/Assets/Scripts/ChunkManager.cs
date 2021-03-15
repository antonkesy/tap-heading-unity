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

public class ChunkManager : MonoBehaviour
{
    [SerializeField] private GameObject coinPrefab;
    [SerializeField] private float coinSpawnProbability;
    private GameObject _coin;

    private bool _isRight;

    internal void SetUp(Transform parentTransform)
    {
        //TODO("remove instantiate")
        _coin = Instantiate(coinPrefab, parentTransform);
    }

    internal void SpawnCoin(Vector3 position, bool isRight)
    {
        _isRight = isRight;
        if (coinSpawnProbability > Random.Range(0, 1f))
        {
            _coin.transform.position = position;
            _coin.SetActive(true);
        }
        else
        {
            _coin.SetActive(false);
        }
    }

    internal void DestroyCall()
    {
        Destroy(_coin);
    }

    internal void MoveOutCall(float duration)
    {
        DestroyCall();
        StartCoroutine(MoveOut(duration));
    }

    private IEnumerator MoveOut(float duration)
    {
        var time = 0f;
        var targetPosition = new Vector3((_isRight ? 15f : -15f), transform.position.y, transform.position.z);
        while (time < duration)
        {
            transform.position = Vector3.LerpUnclamped(transform.position, targetPosition, time / duration);
            time += Time.deltaTime;
            yield return null;
            if (!(Mathf.Abs(targetPosition.x) - Mathf.Abs(transform.position.x) < 1)) continue;
            transform.position = targetPosition;
            break;
        }
    }
}