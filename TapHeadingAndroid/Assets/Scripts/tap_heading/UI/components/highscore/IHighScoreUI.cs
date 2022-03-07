using tap_heading.UI.utility;
using tap_heading.UI.utility.Fade;

namespace tap_heading.UI.components.highscore
{
    public interface IHighScoreUI : IFader
    {
        public void FadeInNewHighScore(float duration);
    }
}