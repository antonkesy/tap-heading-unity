using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class ChunkManager : MonoBehaviour
{
    [SerializeField] private GameObject coinPrefab;
    [SerializeField] private float coinSpawnProbability;
    [SerializeField] private float coinOffsetToBar;


    [SerializeField] private GameObject barPrefab;
  
    private float _speed = 1f;

    internal void SetUp(float xOffset, bool isRight, float speed)
    {
        _speed = speed;
        //TODO("cleanup")
        var parentTransform = transform;
        var barPosition = parentTransform.position;
        barPosition.x += isRight ? xOffset : -xOffset;
        parentTransform.position = barPosition;
        Instantiate(barPrefab, parentTransform);
        if (coinSpawnProbability <= Random.Range(0, 1f))
        {
            var coin = Instantiate(coinPrefab, parentTransform);
            coin.transform.localPosition +=
                (isRight ? Vector3.left : Vector3.right) * (barPrefab.transform.localScale.x / 2f + coinOffsetToBar);
        }
    }

    private void Update()
    {
        gameObject.transform.position += Vector3.down * (_speed * Time.deltaTime);
    }

    public void DestroyCall()
    {
        Destroy(gameObject);
    }

    public void UpdateSpeed(float chunkSpeed)
    {
        _speed = chunkSpeed;
    }
}