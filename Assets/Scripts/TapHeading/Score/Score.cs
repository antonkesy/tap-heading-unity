using TapHeading.Services;
using TapHeading.Services.Google;
using TapHeading.Settings;
using UnityEngine;

namespace TapHeading.Score
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
        }

        private void LoadHighScore()
        {
            GooglePlayServicesManager.Instance.GetHighScore(this);
        }

        public void Add(int value)
        {
            _score += value;
            _listener?.OnScoreUpdate(_score);
            IsNewHighScore();
        }

        /// <summary>
        /// Reports the run to Play Games. Called once per run, not per coin.
        /// </summary>
        public void Submit()
        {
            GooglePlayServicesManager.Instance.SubmitScore(_score);
            GooglePlayServicesManager.Instance.CheckAchievement(_score);
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
            //the leaderboard can be behind the local score (fresh account, offline runs)
            _highScore = Mathf.Max(playerScoreValue, _settings.GetLocalHighScore());
            _settings.SetLocalHighScore(_highScore);
            _listener.OnNewHighScore(_highScore);
        }

        public void OnSignInFailed()
        {
            _highScore = _settings.GetLocalHighScore();
            _listener.OnNewHighScore(_highScore);
        }

        private void IsNewHighScore()
        {
            if (_score <= _highScore)
                return;

            _isHighScore = true;
            _highScore = _score;
            _settings.SetLocalHighScore(_highScore);
            _listener?.OnNewHighScore(_highScore);
        }
    }
}
