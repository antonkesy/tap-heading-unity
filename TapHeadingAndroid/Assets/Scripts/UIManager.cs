using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;
    [Header("Play")] [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI scoreTextShadow;
    [SerializeField] private Button pauseButton;
    [SerializeField] private Button resumeButton;
    [Header("Menu")] [SerializeField] private GameObject menu;
    [SerializeField] private TextMeshProUGUI highScoreText;
    [SerializeField] private TextMeshProUGUI gameTitleText;
    [SerializeField] private TextMeshProUGUI tapToStartText;


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
        scoreText.gameObject.SetActive(true);
        scoreTextShadow.gameObject.SetActive(true);
        pauseButton.gameObject.SetActive(true);
        resumeButton.gameObject.SetActive(false);
        menu.SetActive(false);
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
        menu.SetActive(true);
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
        menu.SetActive(true);
        tapToStartText.text = "TAP TO START";
        gameTitleText.gameObject.SetActive(true);
        highScoreText.gameObject.SetActive(false);
    }
}