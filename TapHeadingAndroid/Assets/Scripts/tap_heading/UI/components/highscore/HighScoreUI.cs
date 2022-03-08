using tap_heading.UI.utility.Transition;
using tap_heading.UI.utility.Transition.Fade;
using UnityEngine;

namespace tap_heading.UI.components.highscore
{
    public class HighScoreUI : MonoBehaviour, IHighScoreUI, ITransition

    {
        [SerializeField] private UIFader newHighScore;
        [SerializeField] private UIFader highScore;

        public void SetScore(int score)
        {
            //TODO 
        }

        public void FadeInNewHighScore(float duration)
        {
            newHighScore.FadeIn();
        }

        public void In()
        {
            highScore.FadeIn();
        }

        public void Out()
        {
            newHighScore.FadeOut();
            highScore.FadeOut();
        }
    }
}