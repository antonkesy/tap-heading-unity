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