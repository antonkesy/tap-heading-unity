using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI scoreTextShadow;
    [SerializeField] private Button pauseButton;
    [SerializeField] private Button resumeButton;


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
}