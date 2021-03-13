using System;
using System.Collections;
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
    [SerializeField] private Button pauseButton;
    [SerializeField] private Button resumeButton;
    [Header("Menu")] [SerializeField] private GameObject menu;
    [SerializeField] private UIMenuManager menuManager;
    [SerializeField] private TextMeshProUGUI highScoreText;
    [SerializeField] private TextMeshProUGUI gameTitleText;
    private UIFader _gameTitleTextFader;
    [SerializeField] private TextMeshProUGUI tapToStartText;

    [SerializeField] private GameObject aboutPanel;

    [SerializeField] private GameObject soundOffButton;
    [SerializeField] private GameObject soundOnButton;

    private bool _isPlaying;
    private bool _isSoundOn;

    private void Start()
    {
        _scoreTextFader = scoreText.GetComponent<UIFader>();
        _scoreTextShadowFader = scoreTextShadow.GetComponent<UIFader>();

        _gameTitleTextFader = gameTitleText.GetComponent<UIFader>();

        _isSoundOn = PlayerPrefs.GetInt("soundOn", 1) == 1;
    }

    internal void UpdateScoreText(int newScore)
    {
        scoreText.text = newScore.ToString();
        scoreTextShadow.text = newScore.ToString();
    }

    public void OnClickPauseButton()
    {
        gameManager.OnPause();
        pauseButton.gameObject.SetActive(false);
        resumeButton.gameObject.SetActive(true);
    }

    public void OnClickResumeButton()
    {
        gameManager.OnResume();
        pauseButton.gameObject.SetActive(true);
        resumeButton.gameObject.SetActive(false);
    }

    internal void ShowPlayUI()
    {
        _isPlaying = true;
        aboutPanel.SetActive(false);
        _scoreTextFader.gameObject.SetActive(true);
        _scoreTextShadowFader.gameObject.SetActive(true);
        _scoreTextFader.Fade(true, 1.5f);
        _scoreTextShadowFader.Fade(true, 1.5f);
        menuManager.FadeOutMenu();
        gameTitleText.gameObject.SetActive(false);
        highScoreText.gameObject.SetActive(false);
    }

    internal void ShowReturningMenuUI()
    {
        _isPlaying = false;
        aboutPanel.SetActive(false);
        //Menu
        menuManager.FadeInMenu();
        tapToStartText.text = "TAP TO RESTART";
        gameTitleText.gameObject.SetActive(false);
        highScoreText.gameObject.SetActive(true);
    }

    internal void ShowStartMenuUI()
    {
        _isPlaying = false;
        aboutPanel.SetActive(false);
        //Play
        scoreText.gameObject.SetActive(false);
        scoreTextShadow.gameObject.SetActive(false);
        //Menu
        menuManager.SetSound(_isSoundOn);
        menuManager.FadeInStart();
        StartCoroutine(WaitForStartCallback());
        tapToStartText.text = "TAP TO START";
        menuManager.SlideInGameTitle();
        highScoreText.gameObject.SetActive(false);
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
        aboutPanel.SetActive(!aboutPanel.activeSelf);
        tapToStartText.gameObject.SetActive(!aboutPanel.activeSelf);
    }

    public void OnLeaderboardButtonClick()
    {
        if (_isPlaying) return;
        //TODO()
    }

    public void OnAchievementsButtonClick()
    {
        if (_isPlaying) return;
        //TODO()
    }

    public void OnSoundOnButtonClick()
    {
        if (_isPlaying) return;
        PlayerPrefs.SetInt("soundOn", 0);
        _isSoundOn = false;
        menuManager.SetSound(false);
        soundOffButton.SetActive(true);
        soundOnButton.SetActive(false);
        //TODO()
    }

    public void OnSoundOffButtonClick()
    {
        if (_isPlaying) return;
        PlayerPrefs.SetInt("soundOn", 1);
        soundOffButton.SetActive(false);
        soundOnButton.SetActive(true);
        menuManager.SetSound(true);
        _isSoundOn = true;
        gameManager.SetSound(false);
    }

    public void OnWebsiteButtonClick()
    {
        if (_isPlaying) return;
        Application.OpenURL("https://poorskill.com/");
    }

    public void OnGitHubButtonClick()
    {
        if (_isPlaying) return;
        Application.OpenURL("https://github.com/PoorSkill/tap-heading-unity");
    }

    public void OnYouTubeButtonClick()
    {
        if (_isPlaying) return;
        Application.OpenURL("https://www.youtube.com/channel/UCgMifJ1aQnFFkwGgrxHSPjg");
    }

    public void OnPlayStoreButtonClick()
    {
        if (_isPlaying) return;
        Application.OpenURL("https://play.google.com/store/apps/details?id=com.poorskill.tapheading");
    }

    internal bool isAboutOn()
    {
        var result = aboutPanel.activeSelf;
        aboutPanel.SetActive(false);
        return result;
    }
}