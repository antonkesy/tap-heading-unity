using System;
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