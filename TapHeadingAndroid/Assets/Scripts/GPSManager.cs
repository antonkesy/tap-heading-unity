using GooglePlayGames;
using GooglePlayGames.BasicApi;
using UnityEngine;

// ReSharper disable once InconsistentNaming
/**
 * GooglePlayServices Manager
 */
public static class GPSManager
{
    /**
     * Activates PlayGamesPlatform
     */
    internal static void Activate()
    {
        var config = new PlayGamesClientConfiguration.Builder().Build();

        PlayGamesPlatform.InitializeInstance(config);
        // recommended for debugging:
        PlayGamesPlatform.DebugLogEnabled = true;
        // Activate the Google Play Games platform
        PlayGamesPlatform.Activate();
    }

    /**
     * Returns isAuthenticated from PlayGamesPlatform
     */
    private static bool IsAuthenticated()
    {
        return PlayGamesPlatform.Instance.IsAuthenticated();
    }

    /**
     * Tries to Add HighScore if User is Logged in to GooglePlayPlatform
     */
    internal static void AddHighScore(long highScore)
    {
        if (IsAuthenticated())
        {
            Social.ReportScore(highScore, GPGSIds.leaderboard_high_score, success => { });
        }
    }

    /**
     * Start OnlyOnce Prompt to SignIn to GooglePlayPlatform
     */
    internal static void SignInToGooglePlayServices()
    {
        PlayGamesPlatform.Instance.Authenticate(SignInInteractivity.CanPromptAlways, result => { });
    }

    /**
     * Tries to Report Progress of Achievements to GooglePlayService
     */
    internal static void CheckAchievement(int highScore)
    {
        if (!IsAuthenticated()) return;

        if (highScore == 0)
        {
            Social.ReportProgress(GPGSIds.achievement_oof, .0f, result => { });
        }

        if (highScore >= 100)
        {
            Social.ReportProgress(GPGSIds.achievement_triple_digest, .0f, result => { });
        }

        if (highScore >= 69)
        {
            Social.ReportProgress(GPGSIds.achievement_nice, .0f, result => { });
        }

        if (highScore >= 42)
        {
            Social.ReportProgress(
                GPGSIds.achievement_answer_to_the_ultimate_question_of_life_the_universe_and_everything, .0f, result => { });
        }

        if (highScore >= 10)
        {
            Social.ReportProgress(GPGSIds.achievement_double_digest, .0f, result => { });
        }
    }

    /**
     * Tries to Report Progress of ThankYou Achievement to GooglePlayService
     */
    internal static void ThankYouAchievement()
    {
        if (!IsAuthenticated()) return;
        Social.ReportProgress(GPGSIds.achievement_thank_you, .0f, result => { });
    }

    /**
     * Tries to open LeaderboardUI and log in, if not logged in
     */
    public static void ShowLeaderboardUI()
    {
        if (!IsAuthenticated())
        {
            PlayGamesPlatform.Instance.Authenticate(SignInInteractivity.CanPromptAlways, result =>
            {
                if (result == SignInStatus.Success)

                    PlayGamesPlatform.Instance.ShowLeaderboardUI();
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
    public static void ShowAchievementsUI()
    {
        if (!IsAuthenticated())
        {
            PlayGamesPlatform.Instance.Authenticate(SignInInteractivity.CanPromptAlways, result =>
            {
                if (result == SignInStatus.Success)
                    PlayGamesPlatform.Instance.ShowAchievementsUI();
            });
        }
        else
        {
            PlayGamesPlatform.Instance.ShowAchievementsUI();
        }
    }
}