using System;

namespace tap_heading.Services
{
    public interface IScoreService
    {
        public void SubmitScore(long highScore);
        public void UnlockAchievement(string id);
        public void ShowLeaderBoardUI();
        public void ShowAchievementsUI();
    }
}