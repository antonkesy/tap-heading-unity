using System;

namespace tap_heading.Services
{
    public interface IScoreService
    {
        public void SubmitScore(long highScore);
        public void UnlockAchievement(string id);
        public void ShowLeaderBoardUI(ISignInListener listener);
        public void ShowAchievementsUI(ISignInListener listener);
    }

    public interface ISignInListener
    {
        void OnSignInSuccess(long playerScoreValue);
        void OnSignInFailed();
    }
}