namespace TapHeading.Settings
{
    public interface ISettings
    {
        public int GetTimesOpen();
        public void IncrementTimesOpen();
        public bool IsSingleClick();
        public void SetSingleClick(bool isSingleClick);
        public bool IsSoundOn();
        public void SetSoundOn(bool isOn);
        public void SetLocalHighScore(int value);
        public int GetLocalHighScore();
    }
}
