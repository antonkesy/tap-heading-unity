using System.Collections;
using tap_heading.manager;
using tap_heading.Services.Google;
using tap_heading.UI.utility;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace tap_heading.UI
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField] private ManagerCollector managers;
        [SerializeField] private UIMenuManager menuManager;
        [Header("Play")] [SerializeField] private TextMeshProUGUI scoreText;
        private UIFader _scoreTextFader;
        [SerializeField] private TextMeshProUGUI scoreTextShadow;
        private UIFader _scoreTextShadowFader;
        [Header("Menu")] [SerializeField] private TextMeshProUGUI highScoreText;
        [SerializeField] private TextMeshProUGUI tapToStartText;
        [SerializeField] private GameObject aboutPanel;
        [SerializeField] private Toggle inputToggle;

        private bool _isPlaying;

        private void Awake()
        {
            _scoreTextFader = scoreText.GetComponent<UIFader>();
            _scoreTextShadowFader = scoreTextShadow.GetComponent<UIFader>();
        }

        /**
     * Sets ScoreText/ScoreShadowText 
     */
        internal void UpdateScoreText(int newScore)
        {
            scoreText.text = newScore.ToString();
            scoreTextShadow.text = newScore.ToString();
        }

        /**
     * Sets HighScoreText
     */
        internal void UpdateHighScoreText(int newScore)
        {
            highScoreText.text = $"BEST: {newScore}";
        }

        /**
     * Starts shows UI for Playing (fade in needed elements / fades out not needed)
     */
        internal void ShowPlayUI()
        {
            _isPlaying = true;
            aboutPanel.SetActive(false);
            _scoreTextFader.FadeIn(.15f);
            _scoreTextShadowFader.FadeIn(.15f);
            menuManager.SetSound();
            menuManager.FadeOutMenu();
        }

        internal void ShowReturningMenuUI()
        {
            ShowMenu("TAP TO RESTART");
            menuManager.FadeInMenu();
        }

        /**
     * Shows Menu
     */
        private void ShowMenu(string tapToText)
        {
            _isPlaying = false;
            aboutPanel.SetActive(false);
            menuManager.SetSound();
            tapToStartText.text = tapToText;
        }

        /**
     * Starts shows UI for Start Menu (fade in needed elements / fades out not needed)
     */
        internal void ShowStartMenuUI()
        {
            _scoreTextFader.FadeOut(0);
            _scoreTextShadowFader.FadeOut(0);
            ShowMenu("TAP TO START");
            menuManager.FadeInStart();
            StartCoroutine(WaitForStartCallback());
        }

        /**
     * Waits for GameTitle to slide in and then calls ready to gameManager
     */
        private IEnumerator WaitForStartCallback()
        {
            menuManager.SlideInGameTitle();
            yield return new WaitForSecondsRealtime(2f);
            managers.GetGameManager().ReadyToStartGameCallback();
            yield return null;
        }

        /**
    * Handles Button Click
    */
        public void OnAboutButtonClick()
        {
            if (_isPlaying) return;
            managers.GetAudioManager().PlayUITap();
            Social.ReportProgress(GPGSIds.AchievementThankYou, 0.0f, null);
            aboutPanel.SetActive(!aboutPanel.activeSelf);
            tapToStartText.gameObject.SetActive(!aboutPanel.activeSelf);
            GooglePlayServicesManager.Instance.ThankYouAchievement();
        }

        /**
    * Handles Button Click
    */
        public void OnLeaderboardButtonClick()
        {
            if (_isPlaying) return;
            managers.GetAudioManager().PlayUITap();
            GooglePlayServicesManager.Instance.ShowLeaderBoardUI(null);
        }

        /**
    * Handles Button Click
    */
        public void OnAchievementsButtonClick()
        {
            if (_isPlaying) return;
            managers.GetAudioManager().PlayUITap();
            GooglePlayServicesManager.Instance.ShowAchievementsUI(null);
        }

        /**
    * Handles Button Click
    */
        public void OnSoundOnButtonClick()
        {
            OnSoundButtonClick(true);
        }

        /**
    * Handles Button Click
    */
        public void OnSoundOffButtonClick()
        {
            OnSoundButtonClick(false);
        }

        /*
     * Switches sound on/off buttons and flags
     */
        private void OnSoundButtonClick(bool setOn)
        {
            if (_isPlaying) return;
            managers.GetAudioManager().SetSound(setOn);
            managers.GetAudioManager().PlayUITap();
            managers.GetSettings().SetSoundOn(setOn);
            menuManager.SetSound();
        }

        /**
    * Opens url in Browser
    */
        private void OpenWebsite(string url)
        {
            if (_isPlaying) return;
            managers.GetAudioManager().PlayUITap();
            Application.OpenURL(url);
        }

        /**
    * Handles Button Click
    */
        public void OnWebsiteButtonClick()
        {
            OpenWebsite("https://antonkesy.de/");
        }

        /**
    * Handles Button Click
    */
        public void OnGitHubButtonClick()
        {
            OpenWebsite("https://github.com/antonkesy/tap-heading-unity");
        }

        /**
    * Handles Button Click
    */
        public void OnYouTubeButtonClick()
        {
            OpenWebsite("https://www.youtube.com/channel/UCgMifJ1aQnFFkwGgrxHSPjg");
        }

        /**
    * Handles Button Click
    */
        public void OnPlayStoreButtonClick()
        {
            OpenWebsite("https://play.google.com/store/apps/details?id=de.antonkesy.tapheading");
        }

        /**
     * Returns if About Panel is active
     */
        internal bool isAboutOn()
        {
            var result = aboutPanel.activeSelf;
            aboutPanel.SetActive(false);
            return result;
        }

        /**
     * Fades in NewHighScore Text
     */
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