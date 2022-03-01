using GooglePlayGames;
using GooglePlayGames.BasicApi;
using tap_heading.Settings;
using UnityEngine;

namespace tap_heading.Services.Google
{
    public class GooglePlayServicesManager : IScoreService
    {
        private static GooglePlayServicesManager _instance;


        private GooglePlayServicesManager()
        {
            Activate();
        }

        public static GooglePlayServicesManager Instance => _instance ??= new GooglePlayServicesManager();

        private static void Activate()
        {
            var config = new PlayGamesClientConfiguration.Builder().Build();

            PlayGamesPlatform.InitializeInstance(config);
            PlayGamesPlatform.Activate();
        }

        private bool IsAuthenticated()
        {
            return PlayGamesPlatform.Instance.IsAuthenticated();
        }

        public void SubmitScore(int highScore)
        {
            if (IsAuthenticated())
            {
                Social.ReportScore(highScore, GPGSIds.LeaderboardHighScore, null);
            }
        }

        private void SignInCanceled(ISignInListener listener)
        {
            new PlayerPrefsManager().SetAutoLogin(false);
            listener?.OnSignInFailed();
        }

        private void SignInSuccess(ISignInListener listener)
        {
            new PlayerPrefsManager().SetAutoLogin(true);
            GetHighScore(listener);
        }


        public void CheckAchievement(int highScore)
        {
            if (!IsAuthenticated()) return;

            if (highScore >= 200)
            {
                UnlockAchievement(GPGSIds.AchievementOof);
            }

            if (highScore >= 100)
            {
                UnlockAchievement(GPGSIds.Achievement100);
            }

            if (highScore >= 69)
            {
                UnlockAchievement(GPGSIds.AchievementNice);
            }

            if (highScore >= 50)
            {
                UnlockAchievement(GPGSIds.Achievement50);
            }

            if (highScore >= 42)
            {
                UnlockAchievement(GPGSIds
                    .AchievementAnswerToTheUltimateQuestionOfLifeTheUniverseAndEverything);
            }
        }

        internal void ThankYouAchievement()
        {
            if (!IsAuthenticated()) return;
            UnlockAchievement(GPGSIds.AchievementThankYou);
        }

        public void UnlockAchievement(string id)
        {
            Social.ReportProgress(id, 100.0f, null);
        }

        public void ShowLeaderBoardUI(ISignInListener listener)
        {
            if (!IsAuthenticated())
            {
                PlayGamesPlatform.Instance.Authenticate(SignInInteractivity.CanPromptAlways, result =>
                {
                    switch (result)
                    {
                        case SignInStatus.Canceled:
                            SignInCanceled(listener);
                            break;
                        case SignInStatus.Success:
                            SignInSuccess(listener);
                            PlayGamesPlatform.Instance.ShowLeaderboardUI();
                            break;
                    }
                });
            }
            else
            {
                PlayGamesPlatform.Instance.ShowLeaderboardUI();
            }
        }

        public void ShowAchievementsUI(ISignInListener listener)
        {
            if (!IsAuthenticated())
            {
                PlayGamesPlatform.Instance.Authenticate(SignInInteractivity.CanPromptAlways, result =>
                {
                    switch (result)
                    {
                        case SignInStatus.Canceled:
                            SignInCanceled(listener);
                            break;
                        case SignInStatus.Success:
                            SignInSuccess(listener);
                            PlayGamesPlatform.Instance.ShowAchievementsUI();
                            break;
                    }
                });
            }
            else
            {
                PlayGamesPlatform.Instance.ShowAchievementsUI();
            }
        }

        public void GetHighScore(ISignInListener listener)
        {
            PlayGamesPlatform.Instance.LoadScores(
                GPGSIds.LeaderboardHighScore,
                LeaderboardStart.PlayerCentered,
                1,
                LeaderboardCollection.Public,
                LeaderboardTimeSpan.AllTime,
                data =>
                {
                    if (!data.Valid) return;
                    listener.OnSignInSuccess((int) data.PlayerScore.value);
                });
        }
    }
}