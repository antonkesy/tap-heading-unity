using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

namespace tap_heading.Game.level.obstacle
{
    public class ObstacleManager : MonoBehaviour, IObstacleManager
    {
        private const float CoinOffsetToBar = .25f;
        private const float DeSpawnTime = 4f;
        private const float CoinSpawnProbability = 0.5f;

        private readonly Transform _obstacleTransform;
        private readonly GameObject _coin;
        private readonly IChunkManager.Side _side;

        public ObstacleManager(Transform obstacleTransform, GameObject coin, IChunkManager.Side side)
        {
            _obstacleTransform = obstacleTransform;
            _coin = coin;
            _side = side;
            SetCoin();
        }

        private void SetCoin()
        {
            var halfChunkWidth = _obstacleTransform.localScale.x / 2f;
            var parentChunkPosition = _obstacleTransform.position + Vector3.right *
                (_side == IChunkManager.Side.Right
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
            var position = _obstacleTransform.position;
            var targetPosition = new Vector3((_side == IChunkManager.Side.Right ? 15f : -15f), position.y, position.z);
            while (time < duration)
            {
                _obstacleTransform.position =
                    Vector3.LerpUnclamped(_obstacleTransform.position, targetPosition, time / duration);
                time += Time.deltaTime;
                yield return null;
                //early stop of Lerp
                if (!(Mathf.Abs(targetPosition.x) - Mathf.Abs(_obstacleTransform.position.x) < 1)) continue;
                _obstacleTransform.position = targetPosition;
                break;
            }
        }

        public void DeSpawn()
        {
            HideCoin();
            //StartCoroutine(MoveOut(despawnTime));
        public void SetSide(IObstacleManager.Side side)
        {
            _side = side;
        }
    }
}