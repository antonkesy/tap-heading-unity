using System.Collections;
using UnityEngine;

namespace TapHeading.Game.Level.Obstacle
{
    public class Obstacle : MonoBehaviour, IObstacle
    {
        [SerializeField] private float deSpawnTime = 4f;
        [SerializeField] private Coin[] coins;
        private IObstacle.Side _side;

        public void DeSpawn()
        {
            StartCoroutine(MoveOut(deSpawnTime));
        }

        public void ResetTo(Vector3 position, IObstacle.Side side)
        {
            _side = side;
            transform.position = position;
            SetCoin();
        }

        public void Move(Vector3 moveBy)
        {
            transform.position += moveBy;
        }

        public float GetYPos()
        {
            return transform.position.y;
        }

        private void SetCoin()
        {
            foreach (var coin in coins)
            {
                coin.Reset();
            }
        }

        private IEnumerator MoveOut(float duration)
        {
            var time = 0f;
            var position = transform.position;
            var targetPosition =
                new Vector3(GetDeSpawnPosition(), position.y, position.z);
            while (time < duration)
            {
                transform.position = Vector3.LerpUnclamped(transform.position, targetPosition, time / duration);
                time += Time.deltaTime;
                yield return null;
                if (DeSpawnTargetDistance(targetPosition) < 1)
                {
                    //early stop of Lerp
                    transform.position = targetPosition;
                    break;
                }
            }
        }

        private float DeSpawnTargetDistance(Vector3 targetPosition)
        {
            return Mathf.Abs(targetPosition.x) - Mathf.Abs(transform.position.x);
        }

        private float GetDeSpawnPosition()
        {
            return _side == IObstacle.Side.Right ? 15f : -15f;
        }
    }
}