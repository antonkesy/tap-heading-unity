using UnityEngine;

namespace tap_heading.Game.level.obstacle
{
    public interface IObstacleManager
    {
        enum Side
        {
            Left,
            Right
        }

        public void DeSpawn();

        void SetSide(Side side);
    }
}