using tap_heading.Services;
using tap_heading.Services.Google;
using tap_heading.Settings;

namespace tap_heading.Score
{
    public class Score : IScore, ISignInListener
    {
        private bool _isHighScore;
        private int _highScore;
        private int _score;
        private readonly ISettings _settings;
        private readonly IScoreListener _listener;

        public Score(IScoreListener listener, ISettings settings)
        {
            _settings = settings;
            _listener = listener;
            LoadHighScore();
            listener.OnNewHighScore(_highScore);
        }

        private void LoadHighScore()
        {
            GooglePlayServicesManager.Instance.GetHighScore(this);
        }

        public void Add(int value)
        {
            _score += value;
            GooglePlayServicesManager.Instance.SubmitScore(_score);
            GooglePlayServicesManager.Instance.CheckAchievement(_score);
            _listener?.OnScoreUpdate(_score);
            IsNewHighScore();
        }

        public void Reset()
        {
            _score = 0;
            _isHighScore = false;
            _listener?.OnScoreUpdate(_score);
        }

        public bool IsHighScore()
        {
            return _isHighScore;
        }

        public void OnSignInSuccess(int playerScoreValue)
        {
            _highScore = playerScoreValue;
            _settings.SetLocalHighScore(playerScoreValue);
            _listener.OnScoreUpdate(_highScore);
        }

        public void OnSignInFailed()
        {
            _highScore = _settings.GetLocalHighScore();
            _listener.OnScoreUpdate(_highScore);
        }

        private void IsNewHighScore()
        {
            if (_score <= _highScore) return;

            _isHighScore = true;
            _highScore = _score;
            _listener?.OnNewHighScore(_highScore);
        }
    }
}