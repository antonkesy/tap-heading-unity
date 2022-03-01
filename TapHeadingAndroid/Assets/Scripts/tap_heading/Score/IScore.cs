namespace tap_heading.Score
{
    public interface IScore
    {
        int GetScore();
        void Add(int value);
        void Reset();
    }

    public interface IScoreListener
    {
        void OnNewHighScore(int highScore);
        void OnScoreUpdate(int score);
    }
}