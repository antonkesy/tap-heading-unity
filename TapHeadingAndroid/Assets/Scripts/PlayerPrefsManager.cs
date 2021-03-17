using UnityEngine;

/**
 * Manages Getter/Setter for PlayerPrefs
 */
public static class PlayerPrefsManager
{
    //Keys
    private const string TimesPlayKey = "p";
    private const string TimesOpenKey = "o";
    private const string AutoLoginKey = "l";
    private const string SoundOnKey = "s";
    private const string LocalHighScoreKey = "h";

    /**
     * Returns times game was played
     */
    internal static int GetTimesPlayed()
    {
        return PlayerPrefs.GetInt(TimesPlayKey, 0);
    }

    /**
    * Increments times game play
    */
    internal static void AddTimesPlayed()
    {
        var timesPlayed = PlayerPrefs.GetInt(TimesPlayKey, 0);
        PlayerPrefs.SetInt(TimesPlayKey, ++timesPlayed);
    }

    /**
     * Returns times application opened
     */
    internal static int GetTimesOpen()
    {
        return PlayerPrefs.GetInt(TimesOpenKey, 0);
    }

    /**
     * Increments times application opened
     */
    internal static void AddTimesOpen()
    {
        var timeOpened = PlayerPrefs.GetInt(TimesOpenKey, 0);
        PlayerPrefs.SetInt(TimesOpenKey, ++timeOpened);
    }

    /*
     * Returns isAutoLoginOn
     */
    internal static bool IsAutoLogin()
    {
        return PlayerPrefs.GetInt(AutoLoginKey, 1) == 1;
    }

    /**
     * Sets isAutoLoginOn
     */
    internal static void SetAutoLogin(bool isAutoLogin)
    {
        PlayerPrefs.SetInt(AutoLoginKey, isAutoLogin ? 1 : 0);
    }

    /**
     * Returns if Sound is on
     */
    internal static bool IsSoundOn()
    {
        return PlayerPrefs.GetInt(SoundOnKey, 0) == 0;
    }

    /**
     * Sets soundOn
     */
    internal static void SetSoundOn(bool isOn)
    {
        PlayerPrefs.SetInt(SoundOnKey, isOn ? 0 : 1);
    }


    /**
     * Sets local highScore
     */
    internal static void SetLocalHighScore(int value)
    {
        PlayerPrefs.SetInt(LocalHighScoreKey, value);
    }


    /**
     * Returns localHighScore
     */
    internal static int GetLocalHighScore()
    {
        return PlayerPrefs.GetInt(LocalHighScoreKey, 0);
    }
}