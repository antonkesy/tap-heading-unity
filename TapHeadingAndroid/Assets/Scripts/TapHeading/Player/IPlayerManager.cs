namespace TapHeading.Player
{
    public interface IPlayerManager
    {
        enum Direction
        {
            Left,
            Right
        }

        public void Spawn();
        bool ChangeDirection(Direction left);
        bool ChangeDirection();
        void StartMoving();
    }
}