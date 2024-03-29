﻿namespace TapHeading.Audio
{
    public interface IAudioManager
    {
        public void SetSound(bool isActive);
        public void PlayStartApplication();
        public void PlayCollectCoin();
        public void PlayPlayerDeath();
        public void PlayPlayerTap();
        public void PlayUITap();
        public void PlayNewHighScore();
    }
}