using UnityEngine;

namespace tap_heading.Game.level.obstacle
{
    public interface IObstacle
    {
        enum Side
        {
            Left,
            Right
        }

        public void DeSpawn();

        void Reset(Vector3 position, Side side);
        void Move(Vector3 moveBy);
        float GetYPos();
    }
}