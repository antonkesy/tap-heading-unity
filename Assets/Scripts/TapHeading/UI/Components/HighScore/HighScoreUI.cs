using TapHeading.UI.Components.Text;
using TapHeading.UI.Utility.Transition;
using TapHeading.UI.Utility.Transition.Fade;
using UnityEngine;

namespace TapHeading.UI.Components.HighScore
{
    public class HighScoreUI : MonoBehaviour, IHighScoreUI, ITransition
    {
        [SerializeField] private FaderUI newHighScore;
        [SerializeField] private FaderUI highScore;
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