using System;
using System.Collections;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;
    [Header("Play")] [SerializeField] private TextMeshProUGUI scoreText;
    private UIFader _scoreTextFader;
    [SerializeField] private TextMeshProUGUI scoreTextShadow;
    private UIFader _scoreTextShadowFader;
    [Header("Menu")] [SerializeField] private GameObject menu;
    [SerializeField] private UIMenuManager menuManager;
    [SerializeField] private TextMeshProUGUI highScoreText;
    [SerializeField] private TextMeshProUGUI gameTitleText;
    [SerializeField] private TextMeshProUGUI tapToStartText;

    [SerializeField] private GameObject aboutPanel;

    private bool _isPlaying;
    private bool _isSoundOn;


    [SerializeField] private AudioManager audioManager;

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
        highScoreText.text = "BEST: " + newScore;
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
        _isPlaying = false;
        aboutPanel.SetActive(false);
        //Menu
        menuManager.SetSound(_isSoundOn);
        menuManager.FadeInMenu();
        tapToStartText.text = "TAP TO RESTART";
    }

    internal void ShowStartMenuUI()
    {
        _isPlaying = false;
        aboutPanel.SetActive(false);
        //Play
        _scoreTextFader.Fade(false, 0);
        _scoreTextShadowFader.Fade(false, 0);
        //Menu
        menuManager.SetSound(_isSoundOn);
        menuManager.FadeInStart();
        StartCoroutine(WaitForStartCallback());
        tapToStartText.text = "TAP TO START";
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
        if (!PlayGamesPlatform.Instance.IsAuthenticated())
        {
            PlayGamesPlatform.Instance.Authenticate(SignInInteractivity.CanPromptAlways, result =>
            {
                switch (result)
                {
                    case SignInStatus.Success:
                        PlayGamesPlatform.Instance.ShowLeaderboardUI();
                        return;
                    default:
                        return;
                }
            });
        }
        else
        {
            PlayGamesPlatform.Instance.ShowLeaderboardUI();
        }
    }

    public void OnAchievementsButtonClick()
    {
        if (_isPlaying) return;
        audioManager.PlayTapUI();
        if (!PlayGamesPlatform.Instance.IsAuthenticated())
        {
            PlayGamesPlatform.Instance.Authenticate(SignInInteractivity.CanPromptAlways, result =>
            {
                switch (result)
                {
                    case SignInStatus.Success:
                        PlayGamesPlatform.Instance.ShowAchievementsUI();
                        return;
                    default:
                        return;
                }
            });
        }
        else
        {
            PlayGamesPlatform.Instance.ShowAchievementsUI();
        }
    }

    public void OnSoundOnButtonClick()
    {
        if (_isPlaying) return;
        audioManager.SetSound(true);
        audioManager.PlayTapUI();
        PlayerPrefs.SetInt("soundOff", 0);
        _isSoundOn = false;
        menuManager.SetSound(false);
    }

    public void OnSoundOffButtonClick()
    {
        if (_isPlaying) return;
        audioManager.SetSound(false);
        audioManager.PlayTapUI();
        PlayerPrefs.SetInt("soundOff", 1);
        menuManager.SetSound(true);
        _isSoundOn = true;
    }

    public void OnWebsiteButtonClick()
    {
        if (_isPlaying) return;
        audioManager.PlayTapUI();
        Application.OpenURL("https://poorskill.com/");
    }

    public void OnGitHubButtonClick()
    {
        if (_isPlaying) return;
        audioManager.PlayTapUI();
        Application.OpenURL("https://github.com/PoorSkill/tap-heading-unity");
    }

    public void OnYouTubeButtonClick()
    {
        if (_isPlaying) return;
        audioManager.PlayTapUI();
        Application.OpenURL("https://www.youtube.com/channel/UCgMifJ1aQnFFkwGgrxHSPjg");
    }

    public void OnPlayStoreButtonClick()
    {
        if (_isPlaying) return;
        audioManager.PlayTapUI();
        Application.OpenURL("https://play.google.com/store/apps/details?id=com.poorskill.tapheading");
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