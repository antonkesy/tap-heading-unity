using UnityEngine;

namespace TapHeading.Game.Level.Obstacle
{
    public interface IObstacle
    {
        enum Side
        {
            Left,
            Right
        }

        public void DeSpawn();

        void ResetTo(Vector3 position, Side side);
        void Move(Vector3 moveBy);
        float GetYPos();
    }
}