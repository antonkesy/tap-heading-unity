namespace tap_heading.Score
{
    public interface IScore
    {
        void Add(int value);
        void Reset();
    }

    public interface IScoreListener
    {
        void OnNewHighScore(int highScore);
        void OnScoreUpdate(int score);
    }
}