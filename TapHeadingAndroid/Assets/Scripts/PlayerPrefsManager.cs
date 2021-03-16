using UnityEngine;

public static class PlayerPrefsManager
{
    private const string TimesPlayKey = "p";
    private const string TimesOpenKey = "o";
    private const string AutoLoginKey = "l";
    private const string SoundOnKey = "s";
    private const string LocalHighScoreKey = "h";

    internal static int GetTimesPlayed()
    {
        return PlayerPrefs.GetInt(TimesPlayKey, 0);
    }

    internal static void AddTimesPlayed()
    {
        var timesPlayed = PlayerPrefs.GetInt(TimesPlayKey, 0);
        PlayerPrefs.SetInt(TimesPlayKey, timesPlayed);
    }

    internal static int GetTimesOpen()
    {
        return PlayerPrefs.GetInt(TimesOpenKey, 0);
    }

    internal static void AddTimesOpen()
    {
        var timesPlayed = PlayerPrefs.GetInt(TimesOpenKey, 0);
        PlayerPrefs.SetInt(TimesOpenKey, timesPlayed);
    }

    internal static bool IsAutoLogin()
    {
        return PlayerPrefs.GetInt(AutoLoginKey, 1) == 1;
    }

    internal static void SetAutoLogin(bool isAutoLogin)
    {
        PlayerPrefs.SetInt(AutoLoginKey, isAutoLogin ? 1 : 0);
    }

    internal static bool IsSoundOn()
    {
        return PlayerPrefs.GetInt(SoundOnKey, 0) == 0;
    }

    internal static void SetSoundOn(bool isOn)
    {
        PlayerPrefs.SetInt(SoundOnKey, isOn ? 0 : 1);
    }


    internal static void SetLocalHighScore(int value)
    {
        PlayerPrefs.SetInt(LocalHighScoreKey, value);
    }


    internal static int GetLocalHighScore()
    {
        return PlayerPrefs.GetInt(LocalHighScoreKey, 0);
    }
}