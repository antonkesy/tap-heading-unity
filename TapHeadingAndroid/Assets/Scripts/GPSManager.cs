using GooglePlayGames;
using GooglePlayGames.BasicApi;
using UnityEngine;

// ReSharper disable once InconsistentNaming
public static class GPSManager
{
    internal static void Activate()
    {
        PlayGamesPlatform.Activate();
    }

    private static bool IsAuthenticated()
    {
        return PlayGamesPlatform.Instance.IsAuthenticated();
    }

    internal static void AddHighScore(long highScore)
    {
        if (IsAuthenticated())
        {
            Social.ReportScore(highScore, GPGSIds.leaderboard_high_score, success => { });
        }
    }

    internal static void SignInToGooglePlayServices()
    {
        PlayGamesPlatform.Instance.Authenticate(SignInInteractivity.CanPromptOnce, null);
    }

    internal static void CheckAchievement(int highScore)
    {
        if (!IsAuthenticated()) return;

        if (highScore == 0)
        {
            Social.ReportProgress(GPGSIds.achievement_oof, 0.0f, null);
        }

        if (highScore >= 100)
        {
            Social.ReportProgress(GPGSIds.achievement_triple_digest, 0.0f, null);
        }

        if (highScore >= 69)
        {
            Social.ReportProgress(GPGSIds.achievement_nice, 0.0f, null);
        }

        if (highScore >= 42)
        {
            Social.ReportProgress(
                GPGSIds.achievement_answer_to_the_ultimate_question_of_life_the_universe_and_everything, 0.0f, null);
        }

        if (highScore >= 10)
        {
            Social.ReportProgress(GPGSIds.achievement_double_digest, 0.0f, null);
        }
    }

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