namespace tap_heading.Game.level
{
    public interface ILevelManager
    {
        public void StartFreshLevel();
        public void IncreaseSpeed();
        public void RestartLevel();
        public void EndLevel();
    }
}