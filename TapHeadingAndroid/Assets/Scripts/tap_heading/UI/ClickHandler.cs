using tap_heading.manager;
using tap_heading.Services.Google;
using tap_heading.UI.components.sound;
using UnityEngine;

namespace tap_heading.UI
{
    public class ClickHandler : MonoBehaviour
    {
        [SerializeField] private ManagerCollector managers;
        [SerializeField] private GameObject aboutPanel;
        [SerializeField] private GameObject tapToStartText;
        [SerializeField] private SoundToggleButton soundToggleButton;

        public void OnAboutButtonClick()
        {
            managers.GetAudioManager().PlayUITap();
            Social.ReportProgress(GPGSIds.AchievementThankYou, 0.0f, null);
            aboutPanel.SetActive(!aboutPanel.activeSelf);
            tapToStartText.gameObject.SetActive(!aboutPanel.activeSelf);
            GooglePlayServicesManager.Instance.ThankYouAchievement();
        }


        public void OnLeaderboardButtonClick()
        {
            managers.GetAudioManager().PlayUITap();
            GooglePlayServicesManager.Instance.ShowLeaderBoardUI(null);
        }


        public void OnAchievementsButtonClick()
        {
            managers.GetAudioManager().PlayUITap();
            GooglePlayServicesManager.Instance.ShowAchievementsUI(null);
        }

        public void OnSoundOnButtonClick()
        {
            OnSoundButtonClick(true);
        }

        public void OnSoundOffButtonClick()
        {
            OnSoundButtonClick(false);
        }

        private void OnSoundButtonClick(bool setOn)
        {
            managers.GetSettings().SetSoundOn(setOn);
            managers.GetAudioManager().SetSound(setOn);
            managers.GetAudioManager().PlayUITap();
            soundToggleButton.Toggle();
        }

        private void OpenWebsite(string url)
        {
            managers.GetAudioManager().PlayUITap();
            Application.OpenURL(url);
        }

        public void OnWebsiteButtonClick()
        {
            OpenWebsite("https://antonkesy.de/");
        }

        public void OnGitHubButtonClick()
        {
            OpenWebsite("https://github.com/antonkesy/tap-heading-unity");
        }

        public void OnYouTubeButtonClick()
        {
            OpenWebsite("https://www.youtube.com/channel/UCgMifJ1aQnFFkwGgrxHSPjg");
        }

        public void OnPlayStoreButtonClick()
        {
            OpenWebsite("https://play.google.com/store/apps/details?id=de.antonkesy.tapheading");
        }
    }
}