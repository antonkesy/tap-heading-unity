using UnityEngine;

namespace TapHeading.Settings
{
    public class PlayerPrefsManager : ISettings
    {
        private const string TimesOpenKey = "timesOpenKey";
        private const string SoundOnKey = "soundOnKey";
        private const string SingleClickKey = "singleClickKey";
        private const string LocalHighScoreKey = "localHighScoreKey";

        public int GetTimesOpen()
        {
            return PlayerPrefs.GetInt(TimesOpenKey, 0);
        }

        public void IncrementTimesOpen()
        {
            var timeOpened = PlayerPrefs.GetInt(TimesOpenKey, 0);
            PlayerPrefs.SetInt(TimesOpenKey, ++timeOpened);
        }

        public bool IsSingleClick()
        {
            return PlayerPrefs.GetInt(SingleClickKey, 1) == 1;
        }

        public void SetSingleClick(bool isSingleClick)
        {
            PlayerPrefs.SetInt(SingleClickKey, isSingleClick ? 1 : 0);
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
