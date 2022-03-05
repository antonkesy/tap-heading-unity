using System.Collections;
using tap_heading.manager;
using tap_heading.Services.Google;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace tap_heading.UI
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField] private ManagerCollector managers;
        [SerializeField] private UIMenuManager menuManager;
        [SerializeField] private components.Score.Score score;
        [Header("Menu")] [SerializeField] private TextMeshProUGUI highScoreText;
        [SerializeField] private TextMeshProUGUI tapToStartText;
        [SerializeField] private GameObject aboutPanel;
        [SerializeField] private Toggle inputToggle;

        private bool _isPlaying;

        private void Start()
        {
            score.HideAll();
        }

        internal void UpdateScoreText(int newScore)
        {
            score.UpdateScore(newScore);
        }

        internal void UpdateHighScoreText(int newScore)
        {
            highScoreText.text = $"BEST: {newScore}";
        }


        internal void ShowPlayUI()
        {
            _isPlaying = true;
            aboutPanel.SetActive(false);
            score.ShowPlaying();
            score.FadeIn(.15f);
            menuManager.SetSound();
            menuManager.FadeOutMenu();
        }

        internal void ShowReturningMenuUI()
        {
            ShowMenu("TAP TO RESTART");
            menuManager.FadeInMenu();
        }


        private void ShowMenu(string tapToText)
        {
            _isPlaying = false;
            aboutPanel.SetActive(false);
            menuManager.SetSound();
            score.ShowMenu();
            tapToStartText.text = tapToText;
        }


        internal void ShowStartMenuUI()
        {
            score.FadeOut(0);
            ShowMenu("TAP TO START");
            StartCoroutine(WaitForStartCallback());
        }


        private IEnumerator WaitForStartCallback()
        {
            menuManager.FadeInStart();
            yield return new WaitForSecondsRealtime(2f);
            managers.GetGameManager().ReadyToStartGameCallback();
            yield return null;
        }


        public void OnAboutButtonClick()
        {
            if (_isPlaying) return;
            managers.GetAudioManager().PlayUITap();
            Social.ReportProgress(GPGSIds.AchievementThankYou, 0.0f, null);
            aboutPanel.SetActive(!aboutPanel.activeSelf);
            tapToStartText.gameObject.SetActive(!aboutPanel.activeSelf);
            GooglePlayServicesManager.Instance.ThankYouAchievement();
        }


        public void OnLeaderboardButtonClick()
        {
            if (_isPlaying) return;
            managers.GetAudioManager().PlayUITap();
            GooglePlayServicesManager.Instance.ShowLeaderBoardUI(null);
        }


        public void OnAchievementsButtonClick()
        {
            if (_isPlaying) return;
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
            if (_isPlaying) return;
            managers.GetSettings().SetSoundOn(setOn);
            managers.GetAudioManager().SetSound(setOn);
            managers.GetAudioManager().PlayUITap();
            menuManager.SetSound();
        }

        private void OpenWebsite(string url)
        {
            if (_isPlaying) return;
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

        internal bool isAboutOn()
        {
            var result = aboutPanel.activeSelf;
            aboutPanel.SetActive(false);
            return result;
        }

        internal void FadeInNewHighScore()
        {
            menuManager.FadeInNewHighScore();
        }

        public void ToggleInputSettings()
        {
            managers.GetGameManager().SetSingleClick(inputToggle.isOn);
        }
    }
}