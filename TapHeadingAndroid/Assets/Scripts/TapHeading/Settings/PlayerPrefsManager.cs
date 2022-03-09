using UnityEngine;

namespace TapHeading.Settings
{
    public class PlayerPrefsManager : ISettings
    {
        private const string TimesPlayKey = "timesPlayedKey";
        private const string TimesOpenKey = "timesOpenKey";
        private const string AutoLoginKey = "autoLoginKey";
        private const string SoundOnKey = "soundOnKey";
        private const string LocalHighScoreKey = "localHighScoreKey";

        public int GetTimesPlayed()
        {
            return PlayerPrefs.GetInt(TimesPlayKey, 0);
        }

        public void IncrementTimesPlayed()
        {
            var timesPlayed = PlayerPrefs.GetInt(TimesPlayKey, 0);
            PlayerPrefs.SetInt(TimesPlayKey, ++timesPlayed);
        }

        public int GetTimesOpen()
        {
            return PlayerPrefs.GetInt(TimesOpenKey, 0);
        }

        public void IncrementTimesOpen()
        {
            var timeOpened = PlayerPrefs.GetInt(TimesOpenKey, 0);
            PlayerPrefs.SetInt(TimesOpenKey, ++timeOpened);
        }

        public bool IsAutoLogin()
        {
            return PlayerPrefs.GetInt(AutoLoginKey, 1) == 1;
        }

        public void SetAutoLogin(bool isAutoLogin)
        {
            PlayerPrefs.SetInt(AutoLoginKey, isAutoLogin ? 1 : 0);
        }

        public bool IsSoundOn()
        {
            return PlayerPrefs.GetInt(SoundOnKey, 1) == 1;
        }

        public void SetSoundOn(bool isOn)
        {
            PlayerPrefs.SetInt(SoundOnKey, isOn ? 1 : 0);
        }

        public void SetLocalHighScore(int value)
        {
            PlayerPrefs.SetInt(LocalHighScoreKey, value);
        }

        public int GetLocalHighScore()
        {
            return PlayerPrefs.GetInt(LocalHighScoreKey, 0);
        }
    }
}