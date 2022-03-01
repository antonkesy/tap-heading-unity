using GooglePlayGames;
using GooglePlayGames.BasicApi;
using tap_heading.Game;
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

        private static bool IsAuthenticated()
        {
            return PlayGamesPlatform.Instance.IsAuthenticated();
        }

        public void SubmitScore(long highScore)
        {
            if (IsAuthenticated())
            {
                Social.ReportScore(highScore, GPGSIds.LeaderboardHighScore, null);
            }
        }

        private void SignInToGooglePlayServices()
        {
            PlayGamesPlatform.Instance.Authenticate(SignInInteractivity.CanPromptAlways, result =>
            {
                switch (result)
                {
                    case SignInStatus.Canceled:
                        SignInCanceled();
                        break;
                    case SignInStatus.Success:
                        SignInSuccess();
                        break;
                    default:
                        //ignore
                        break;
                }
            });
        }

        private void SignInCanceled()
        {
            new PlayerPrefsManager().SetAutoLogin(false);
            GameManager.SetHighScoreFromLocal();
        }

        private void SignInSuccess()
        {
            new PlayerPrefsManager().SetAutoLogin(true);
            GameManager.Instance.OverwriteGPSHighScore();
            LoadLeaderboardFromGPS();
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

        public void ShowLeaderBoardUI()
        {
            if (!IsAuthenticated())
            {
                PlayGamesPlatform.Instance.Authenticate(SignInInteractivity.CanPromptAlways, result =>
                {
                    switch (result)
                    {
                        case SignInStatus.Canceled:
                            SignInCanceled();
                            break;
                        case SignInStatus.Success:
                            SignInSuccess();
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

        public void ShowAchievementsUI()
        {
            if (!IsAuthenticated())
            {
                PlayGamesPlatform.Instance.Authenticate(SignInInteractivity.CanPromptAlways, result =>
                {
                    switch (result)
                    {
                        case SignInStatus.Canceled:
                            SignInCanceled();
                            break;
                        case SignInStatus.Success:
                            SignInSuccess();
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

        private static void LoadLeaderboardFromGPS()
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
                    GameManager.SetHighScoreFromGPS(data.PlayerScore.value);
                });
        }
    }
}