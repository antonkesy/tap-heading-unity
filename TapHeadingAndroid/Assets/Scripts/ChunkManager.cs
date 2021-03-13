using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class ChunkManager : MonoBehaviour
{
    [SerializeField] private GameObject coinPrefab;
    [SerializeField] private float coinSpawnProbability;
    private GameObject _coin;

    internal void SetUp(Transform parentTransform)
    {
        _coin = Instantiate(coinPrefab, parentTransform);
    }

    internal void SpawnCoin(Vector3 position)
    {
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

    public void DestroyCall()
    {
        Destroy(_coin);
    }
}