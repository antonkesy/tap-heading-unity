using tap_heading.manager;
using tap_heading.UI.components.highscore;
using UnityEngine;

namespace tap_heading.UI.State
{
    public abstract class UIState : MonoBehaviour
    {
        [SerializeField] protected ManagerCollector managers;
        [SerializeField] protected components.Score.Score score;
        [SerializeField] protected HighScoreUI highScoreUI;
        public abstract void OnEntering();
        public abstract void OnLeaving();

        internal void UpdateScoreText(int newScore)
        {
            score.UpdateScore(newScore);
        }

        internal void UpdateHighScoreText(int newScore)
        {
            highScoreUI.SetScore(newScore);
        }

        internal void FadeInNewHighScore()
        {
            //menuManager.FadeInNewHighScore(1f);
        }
    }
}