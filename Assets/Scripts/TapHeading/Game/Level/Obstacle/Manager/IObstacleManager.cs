namespace TapHeading.Game.Level.Obstacle.Manager
{
    public interface IObstacleManager
    {
        public void SetSpeed(float speed);
        public void Restart();
        public void Stop();
    }
}