namespace TapHeading.Services
{
    public interface IScoreService
    {
        public void SubmitScore(int highScore);
        public void UnlockAchievement(string id);
        public void ShowLeaderBoardUI(ISignInListener listener);
        public void ShowAchievementsUI(ISignInListener listener);
        public void GetHighScore(ISignInListener listener);
    }

    public interface ISignInListener
    {
        void OnSignInSuccess(int playerScoreValue);
        void OnSignInFailed();
    }
}