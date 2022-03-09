namespace tap_heading.Settings
{
    public interface ISettings
    {
        public int GetTimesPlayed();
        public void IncrementTimesPlayed();
        public int GetTimesOpen();
        public void IncrementTimesOpen();
        public bool IsAutoLogin();
        public void SetAutoLogin(bool isAutoLogin);
        public bool IsSoundOn();
        public void SetSoundOn(bool isOn);
        public void SetLocalHighScore(int value);
        public int GetLocalHighScore();
    }
}