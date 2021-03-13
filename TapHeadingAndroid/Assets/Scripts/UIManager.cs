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

    private void Start()
    {
        _scoreTextFader = scoreText.GetComponent<UIFader>();
        _scoreTextShadowFader = scoreTextShadow.GetComponent<UIFader>();

        _gameTitleTextFader = gameTitleText.GetComponent<UIFader>();
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
        _scoreTextFader.Fade(true, .5f);
        _scoreTextShadowFader.Fade(true, .5f);
        pauseButton.gameObject.SetActive(true);
        resumeButton.gameObject.SetActive(false);
        menuManager.FadeOutMenu();
        gameTitleText.gameObject.SetActive(false);
        highScoreText.gameObject.SetActive(false);
    }

    internal void ShowReturningMenuUI()
    {
        //Play
        scoreText.gameObject.SetActive(true);
        scoreTextShadow.gameObject.SetActive(true);
        pauseButton.gameObject.SetActive(false);
        resumeButton.gameObject.SetActive(false);
        //Menu
        menuManager.FadeInMenu();
        tapToStartText.text = "TAP TO RESTART";
        gameTitleText.gameObject.SetActive(false);
        highScoreText.gameObject.SetActive(true);
    }

    internal void ShowStartMenuUI()
    {
        //Play
        scoreText.gameObject.SetActive(false);
        scoreTextShadow.gameObject.SetActive(false);
        pauseButton.gameObject.SetActive(false);
        resumeButton.gameObject.SetActive(false);
        //Menu
        menuManager.FadeInStart();
        tapToStartText.text = "TAP TO START";
        menuManager.SlideInGameTitle();
        highScoreText.gameObject.SetActive(false);
    }

    public void OnHomeButtonClick()
    {
        ShowReturningMenuUI();
        gameManager.ResetToStart();
    }
}