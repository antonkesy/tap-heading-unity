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

public class UIManager : MonoBehaviour
{
    [Header("Manager")] [SerializeField] private GameManager gameManager;
    [SerializeField] private UIMenuManager menuManager;
    [SerializeField] private AudioManager audioManager;
    [Header("Play")] [SerializeField] private TextMeshProUGUI scoreText;
    private UIFader _scoreTextFader;
    [SerializeField] private TextMeshProUGUI scoreTextShadow;
    private UIFader _scoreTextShadowFader;
    [Header("Menu")] [SerializeField] private TextMeshProUGUI highScoreText;
    [SerializeField] private TextMeshProUGUI tapToStartText;
    [SerializeField] private GameObject aboutPanel;

    private bool _isPlaying;
    private bool _isSoundOn;


    private void Start()
    {
        _scoreTextFader = scoreText.GetComponent<UIFader>();
        _scoreTextShadowFader = scoreTextShadow.GetComponent<UIFader>();
        _isSoundOn = PlayerPrefs.GetInt("soundOff", 1) == 1;
        UpdateHighScoreText(PlayerPrefs.GetInt("highScore", 0));
    }

    internal void UpdateScoreText(int newScore)
    {
        scoreText.text = newScore.ToString();
        scoreTextShadow.text = newScore.ToString();
    }

    internal void UpdateHighScoreText(int newScore)
    {
        highScoreText.text = $"BEST: {newScore}";
    }

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

    private void ShowMenu(string tapToText)
    {
        _isPlaying = false;
        aboutPanel.SetActive(false);
        menuManager.SetSound(_isSoundOn);
        tapToStartText.text = tapToText;
    }

    internal void ShowStartMenuUI()
    {
        _scoreTextFader.Fade(false, 0);
        _scoreTextShadowFader.Fade(false, 0);
        ShowMenu("TAP TO START");
        menuManager.FadeInStart();
        StartCoroutine(WaitForStartCallback());
        menuManager.SlideInGameTitle();
    }

    private IEnumerator WaitForStartCallback()
    {
        yield return new WaitForSecondsRealtime(1.5f);
        gameManager.ReadyToStartGameCallback();
        yield return null;
    }


    public void OnAboutButtonClick()
    {
        if (_isPlaying) return;
        audioManager.PlayTapUI();
        Social.ReportProgress(GPGSIds.achievement_thank_you, 0.0f, null);
        aboutPanel.SetActive(!aboutPanel.activeSelf);
        tapToStartText.gameObject.SetActive(!aboutPanel.activeSelf);
    }

    public void OnLeaderboardButtonClick()
    {
        if (_isPlaying) return;
        audioManager.PlayTapUI();
        GPSManager.ShowLeaderboardUI();
    }

    public void OnAchievementsButtonClick()
    {
        if (_isPlaying) return;
        audioManager.PlayTapUI();
        GPSManager.ShowAchievementsUI();
    }

    public void OnSoundOnButtonClick()
    {
        OnSoundButtonClick(true);
    }

    public void OnSoundOffButtonClick()
    {
        OnSoundButtonClick(false);
    }

    private void OnSoundButtonClick(bool soundOn)
    {
        if (_isPlaying) return;
        audioManager.SetSound(soundOn);
        audioManager.PlayTapUI();
        PlayerPrefs.SetInt("soundOff", soundOn ? 0 : 1);
        _isSoundOn = !soundOn;
        menuManager.SetSound(!soundOn);
    }

    private void OpenWebsite(string url)
    {
        if (_isPlaying) return;
        audioManager.PlayTapUI();
        Application.OpenURL(url);
    }

    public void OnWebsiteButtonClick()
    {
        OpenWebsite("https://poorskill.com/");
    }

    public void OnGitHubButtonClick()
    {
        OpenWebsite("https://github.com/PoorSkill/tap-heading-unity");
    }

    public void OnYouTubeButtonClick()
    {
        OpenWebsite("https://www.youtube.com/channel/UCgMifJ1aQnFFkwGgrxHSPjg");
    }

    public void OnPlayStoreButtonClick()
    {
        OpenWebsite("https://play.google.com/store/apps/details?id=com.poorskill.tapheading");
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
}