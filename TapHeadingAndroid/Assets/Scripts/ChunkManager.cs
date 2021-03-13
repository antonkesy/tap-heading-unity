using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class ChunkManager : MonoBehaviour
{
    [SerializeField] private GameObject coinPrefab;
    [SerializeField] private float coinSpawnProbability;

    internal void SpawnCoin(Transform parentTransform, float xPosition)
    {
        if (coinSpawnProbability > Random.Range(0, 1f))
        {
            var coin = Instantiate(coinPrefab, parentTransform);
            coin.transform.localPosition += Vector3.right * xPosition;
        }
    }
}