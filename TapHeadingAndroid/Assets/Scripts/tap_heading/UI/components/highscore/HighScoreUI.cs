using tap_heading.UI.components.Text;
using tap_heading.UI.utility.Transition;
using tap_heading.UI.utility.Transition.Fade;
using UnityEngine;

namespace tap_heading.UI.components.highscore
{
    public class HighScoreUI : MonoBehaviour, IHighScoreUI, ITransition
    {
        [SerializeField] private UIFader newHighScore;
        [SerializeField] private UIFader highScore;
        [SerializeField] private ShadowText text;

        public void SetScore(int score)
        {
            text.SetText(score.ToString());
        }

        public void FadeInNewHighScore(float duration)
        {
            newHighScore.In();
        }

        public void In()
        {
            highScore.In();
        }

        public void Out()
        {
            newHighScore.Out();
            highScore.Out();
        }
    }
}