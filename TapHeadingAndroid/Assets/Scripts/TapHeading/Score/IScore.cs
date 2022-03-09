namespace TapHeading.Score
{
    public interface IScore
    {
        void Add(int value);
        void Reset();
        bool IsHighScore();
    }

    public interface IScoreListener
    {
        void OnNewHighScore(int highScore);
        void OnScoreUpdate(int score);
    }
}