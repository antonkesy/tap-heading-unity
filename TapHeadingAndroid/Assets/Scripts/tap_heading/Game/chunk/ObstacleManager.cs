using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

namespace tap_heading.Game.chunk
{
    public class ObstacleManager : MonoBehaviour, IChunkManager
    {
        [Header("Coin")] [SerializeField] private GameObject coinGameObject;
        [SerializeField] private float coinSpawnProbability;

        private IChunkManager.Side _side;

        [SerializeField] private float despawnTime = 4f;

        public void SpawnCoin(Vector3 position, IChunkManager.Side side)
        {
            _side = side;
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

        private void HideCoin()
        {
            coinGameObject.SetActive(false);
        }

        private IEnumerator MoveOut(float duration)
        {
            var time = 0f;
            var position = transform.position;
            var targetPosition = new Vector3((_side == IChunkManager.Side.Right ? 15f : -15f), position.y, position.z);
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

        public void MoveOut()
        {
            HideCoin();
            StartCoroutine(MoveOut(despawnTime));
        }
    }
}