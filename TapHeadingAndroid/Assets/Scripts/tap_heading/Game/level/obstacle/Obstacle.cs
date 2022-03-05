using System.Collections;
using System.Security.Cryptography;
using UnityEngine;

namespace tap_heading.Game.level.obstacle
{
    public class Obstacle : MonoBehaviour, IObstacle
    {
        private const float CoinOffsetToBar = .25f;
        private const float DeSpawnTime = 4f;
        private const float CoinSpawnProbability = 0.5f;

        private GameObject _coin;
        private IObstacle.Side _side;

        private void Start()
        {
            _coin = GetComponentInChildren<CircleCollider2D>().gameObject;
        }

        private void SetCoin()
        {
            var halfChunkWidth = transform.localScale.x / 2f;
            var parentChunkPosition = transform.position + Vector3.right *
                (_side == IObstacle.Side.Right
                    ? -CoinOffsetToBar - halfChunkWidth
                    : CoinOffsetToBar + halfChunkWidth);


            if (CoinSpawnProbability > Random.Range(0, 1f))
            {
                _coin.transform.position = parentChunkPosition;
                _coin.SetActive(true);
            }
            else
            {
                _coin.SetActive(false);
            }
        }

        private void HideCoin()
        {
            _coin.SetActive(false);
        }

        private IEnumerator MoveOut(float duration)
        {
            var time = 0f;
            var position = transform.position;
            var targetPosition =
                new Vector3((_side == IObstacle.Side.Right ? 15f : -15f), position.y, position.z);
            while (time < duration)
            {
                transform.position =
                    Vector3.LerpUnclamped(transform.position, targetPosition, time / duration);
                time += Time.deltaTime;
                yield return null;
                //early stop of Lerp
                if (!(Mathf.Abs(targetPosition.x) - Mathf.Abs(transform.position.x) < 1)) continue;
                transform.position = targetPosition;
                break;
            }
        }

        public void DeSpawn()
        {
            HideCoin();
            StartCoroutine(MoveOut(DeSpawnTime));
        }

        public void SetSide(IObstacle.Side side)
        {
            _side = side;
        }
    }
}