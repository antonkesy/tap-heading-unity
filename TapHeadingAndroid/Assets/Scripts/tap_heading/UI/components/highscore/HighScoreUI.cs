using tap_heading.UI.utility;
using UnityEngine;

namespace tap_heading.UI.components.highscore
{
    public class HighScoreUI : MonoBehaviour, IHighScoreUI
    {
        [SerializeField] private UIFader newHighScore;
        [SerializeField] private UIFader highScore;

        public void SetScore(int score)
        {
            //TODO 
        }

        public void FadeInNewHighScore(float duration)
        {
            newHighScore.FadeIn(duration);
        }

        public void FadeIn(float duration)
        {
            highScore.FadeIn(duration);
        }

        public void FadeOut(float duration)
        {
            newHighScore.FadeOut(duration);
            highScore.FadeOut(duration);
        }
    }
}