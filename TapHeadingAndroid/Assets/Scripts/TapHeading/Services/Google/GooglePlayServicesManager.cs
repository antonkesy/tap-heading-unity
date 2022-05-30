using GooglePlayGames;
using GooglePlayGames.BasicApi;
using UnityEngine;

namespace TapHeading.Services.Google
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
                Social.ReportScore(highScore, GPGSIds.leaderboard_high_score, null);
            }
        }

        private void SignInCanceled(ISignInListener listener)
        {
            listener?.OnSignInFailed();
        }

        private void SignInSuccess(ISignInListener listener)
        {
            GetHighScore(listener);
        }


        public void CheckAchievement(int highScore)
        {
            if (!IsAuthenticated()) return;

            if (highScore >= 200)
            {
                UnlockAchievement(GPGSIds.achievement_oof);
            }

            if (highScore >= 100)
            {
                UnlockAchievement(GPGSIds.achievement_100);
            }

            if (highScore >= 69)
            {
                UnlockAchievement(GPGSIds.achievement_nice);
            }

            if (highScore >= 50)
            {
                UnlockAchievement(GPGSIds.achievement_50);
            }

            if (highScore >= 42)
            {
                UnlockAchievement(GPGSIds
                    .achievement_answer_to_the_ultimate_question_of_life_the_universe_and_everything);
            }
        }

        internal void ThankYouAchievement()
        {
            if (!IsAuthenticated()) return;
            UnlockAchievement(GPGSIds.achievement_thank_you);
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
                GPGSIds.leaderboard_high_score,
                LeaderboardStart.PlayerCentered,
                1,
                LeaderboardCollection.Public,
                LeaderboardTimeSpan.AllTime,
                data =>
                {
                    if (data.Valid)
                    {
                        listener?.OnSignInSuccess((int) data.PlayerScore.value);
                    }
                    else
                    {
                        listener?.OnSignInFailed();
                    }
                });
        }

        public void SignIn()
        {
            PlayGamesPlatform.Instance.Authenticate(SignInInteractivity.CanPromptOnce, result => { });
        }
    }
}