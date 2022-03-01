using GooglePlayGames;
using GooglePlayGames.BasicApi;
using tap_heading.Game;
using tap_heading.Settings;
using UnityEngine;

// ReSharper disable once InconsistentNaming
namespace tap_heading.Services.Google
{
    /**
 * GooglePlayServices Manager
 */
    public class GPSManager : IScoreService
    {
        private static GPSManager _instance;

        private GPSManager()
        {
            Activate();
        }

        public static GPSManager Instance => _instance ??= new GPSManager();

        private static void Activate()
        {
            var config = new PlayGamesClientConfiguration.Builder().Build();

            PlayGamesPlatform.InitializeInstance(config);
            PlayGamesPlatform.Activate();
        }

        /**
     * Returns isAuthenticated from PlayGamesPlatform
     */
        private static bool IsAuthenticated()
        {
            return PlayGamesPlatform.Instance.IsAuthenticated();
        }

        public void SubmitScore(long highScore)
        {
            if (IsAuthenticated())
            {
                Social.ReportScore(highScore, GPGSIds.leaderboard_high_score, null);
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
            PlayerPrefsManager.SetAutoLogin(false);
            GameManager.SetHighScoreFromLocal();
        }

        private void SignInSuccess()
        {
            PlayerPrefsManager.SetAutoLogin(true);
            GameManager.OverwriteGPSHighScore();
            LoadLeaderboardFromGPS();
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

        /**
     * Tries to Report Progress of ThankYou Achievement to GooglePlayService
     */
        internal void ThankYouAchievement()
        {
            if (!IsAuthenticated()) return;
            UnlockAchievement(GPGSIds.achievement_thank_you);
        }

        public void UnlockAchievement(string id)
        {
            Social.ReportProgress(id, 100.0f, null);
        }

        /**
     * Tries to open LeaderboardUI and log in, if not logged in
     */
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

        /**
     * Tries to open AchievementsUI and log in, if not logged in
     */
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

        // ReSharper disable once InconsistentNaming
        /**
     * Tries to load highScore of user from google play services and sets it as local highscore  
     */
        private void LoadLeaderboardFromGPS()
        {
            PlayGamesPlatform.Instance.LoadScores(
                GPGSIds.leaderboard_high_score,
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