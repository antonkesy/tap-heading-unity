using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText;

    internal void UpdateScoreText(int newScore)
    {
        scoreText.text = newScore.ToString();
    }
}
