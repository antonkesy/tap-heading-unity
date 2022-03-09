using TapHeading.Manager;
using TapHeading.Services.Google;
using TapHeading.UI.Components.About;
using TapHeading.UI.Components.Sound;
using UnityEngine;

namespace TapHeading.UI
{
    public class ClickHandler : MonoBehaviour
    {
        [SerializeField] private AboutUI aboutUI;
        [SerializeField] private ManagerCollector managers;
        [SerializeField] private SoundToggleButton soundToggleButton;

        public void OnAboutButtonClick()
        {
            managers.GetAudioManager().PlayUITap();
            if (aboutUI.IsOpen())
            {
                aboutUI.Close();
            }
            else
            {
                aboutUI.Open();
            }
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
            OnSoundButtonClick(false);
        }

        public void OnSoundOffButtonClick()
        {
            OnSoundButtonClick(true);
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

        public void OnGitHubButtonClick()
        {
            OpenWebsite("https://github.com/antonkesy/tap-heading-unity");
        }

        public void OnPlayStoreButtonClick()
        {
            OpenWebsite("https://play.google.com/store/apps/details?id=de.antonkesy.tapheading");
        }
    }
}