/*
MIT License
Copyright (c) 2021 Anton Kesy
Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:
The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.
THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
 */

using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("Manager")] [SerializeField] private GameManager gameManager;
    [SerializeField] private UIMenuManager menuManager;
    private IAudioManager _audioManager;
    [Header("Play")] [SerializeField] private TextMeshProUGUI scoreText;
    private UIFader _scoreTextFader;
    [SerializeField] private TextMeshProUGUI scoreTextShadow;
    private UIFader _scoreTextShadowFader;
    [Header("Menu")] [SerializeField] private TextMeshProUGUI highScoreText;
    [SerializeField] private TextMeshProUGUI tapToStartText;
    [SerializeField] private GameObject aboutPanel;
    [SerializeField] private Toggle inputToggle;

    private bool _isPlaying;
    private bool _isSoundOn;


    private void Start()
    {
        _audioManager = gameManager.GetComponent<IAudioManager>();
        _scoreTextFader = scoreText.GetComponent<UIFader>();
        _scoreTextShadowFader = scoreTextShadow.GetComponent<UIFader>();
        _isSoundOn = !PlayerPrefsManager.IsSoundOn();
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
        _scoreTextFader.Fade(true, .15f);
        _scoreTextShadowFader.Fade(true, .15f);
        menuManager.SetSound(_isSoundOn);
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
        menuManager.SetSound(_isSoundOn);
        tapToStartText.text = tapToText;
    }

    /**
     * Starts shows UI for Start Menu (fade in needed elements / fades out not needed)
     */
    internal void ShowStartMenuUI()
    {
        _scoreTextFader.Fade(false, 0);
        _scoreTextShadowFader.Fade(false, 0);
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
        gameManager.ReadyToStartGameCallback();
        yield return null;
    }

    /**
    * Handles Button Click
    */
    public void OnAboutButtonClick()
    {
        if (_isPlaying) return;
        _audioManager.PlayUITap();
        Social.ReportProgress(GPGSIds.achievement_thank_you, 0.0f, null);
        aboutPanel.SetActive(!aboutPanel.activeSelf);
        tapToStartText.gameObject.SetActive(!aboutPanel.activeSelf);
        GPSManager.ThankYouAchievement();
    }

    /**
    * Handles Button Click
    */
    public void OnLeaderboardButtonClick()
    {
        if (_isPlaying) return;
        _audioManager.PlayUITap();
        GPSManager.ShowLeaderboardUI();
    }

    /**
    * Handles Button Click
    */
    public void OnAchievementsButtonClick()
    {
        if (_isPlaying) return;
        _audioManager.PlayUITap();
        GPSManager.ShowAchievementsUI();
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
    private void OnSoundButtonClick(bool soundOn)
    {
        if (_isPlaying) return;
        _audioManager.SetSound(soundOn);
        _audioManager.PlayUITap();
        PlayerPrefsManager.SetSoundOn(soundOn);
        _isSoundOn = !soundOn;
        menuManager.SetSound(!soundOn);
    }

    /**
    * Opens url in Browser
    */
    private void OpenWebsite(string url)
    {
        if (_isPlaying) return;
        _audioManager.PlayUITap();
        Application.OpenURL(url);
    }

    /**
    * Handles Button Click
    */
    public void OnWebsiteButtonClick()
    {
        OpenWebsite("https://poorskill.com/");
    }

    /**
    * Handles Button Click
    */
    public void OnGitHubButtonClick()
    {
        OpenWebsite("https://github.com/PoorSkill/tap-heading-unity");
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
        OpenWebsite("https://play.google.com/store/apps/details?id=com.poorskill.tapheading");
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
        gameManager.isSingleClick = inputToggle.isOn;
    }
}