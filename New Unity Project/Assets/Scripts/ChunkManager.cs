using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChunkManager : MonoBehaviour
{
    [SerializeField] private GameObject coinPrefab;
    [SerializeField] private float coinSpawnProbability;
    [SerializeField] private float coinOffsetToBar;
    

    [SerializeField] private GameObject barPrefab;
    private float _destroyPosition;

    private float _speed = 1f;

    internal void SetUp(float xOffset, bool isRight, float speed, float destroyPosition)
    {
        _destroyPosition = destroyPosition;
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
        if (gameObject.transform.position.y <= _destroyPosition)
        {
            Destroy(gameObject);
        }
    }
}