using System.Collections;
using UnityEngine;

namespace TapHeading.Game.Level.Obstacle
{
    public class Obstacle : MonoBehaviour, IObstacle
    {
        private const float DeSpawnTime = 4f;

        [SerializeField] private Coin[] coins;
        private IObstacle.Side _side;

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
            StartCoroutine(MoveOut(DeSpawnTime));
        }

        public void Reset(Vector3 position, IObstacle.Side side)
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
    }
}