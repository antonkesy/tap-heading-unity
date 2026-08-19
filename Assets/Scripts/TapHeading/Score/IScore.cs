namespace TapHeading.Score
{
    public interface IScore
    {
        void Add(int value);
        void Submit();
        void Reset();
        bool IsHighScore();
    }

    public interface IScoreListener
    {
        void OnNewHighScore(int highScore);
        void OnScoreUpdate(int score);
    }
}
