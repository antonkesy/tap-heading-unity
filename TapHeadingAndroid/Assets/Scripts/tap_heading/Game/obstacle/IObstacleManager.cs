using UnityEngine;

namespace tap_heading.Game.chunk
{
    public interface IChunkManager
    {
        enum Side
        {
            Left,
            Right
        }

        public void MoveOut();

        public void SpawnCoin(Vector3 position, Side side);
    }
}