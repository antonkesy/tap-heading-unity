using UnityEngine;

namespace tap_heading.Game.level.obstacle
{
    public interface IChunkManager
    {
        enum Side
        {
            Left,
            Right
        }

        public void DeSpawn();

    }
}