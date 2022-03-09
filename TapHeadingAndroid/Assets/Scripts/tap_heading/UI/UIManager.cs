using tap_heading.UI.components.About;
using tap_heading.UI.components.highscore;
using tap_heading.UI.State;
using tap_heading.UI.State.States;
using UnityEngine;

namespace tap_heading.UI
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField] private AboutUI aboutPanel;
        [SerializeField] private components.Score.Score score;
        [SerializeField] private HighScoreUI highScore;

        private StateMachine _state;

        private void Awake()
        {
            var start = GetComponent<StartUI>();
            var menu = GetComponent<MenuUI>();
            var playing = GetComponent<PlayingUI>();
            _state = new StateMachine(start, menu, playing);
            _state.ShowStart();
        }


        internal bool CancelAbout()
        {
            if (!aboutPanel.IsOpen()) return false;
            
            aboutPanel.Close();
            return true;
        }

        public void ShowMenu()
        {
            _state.ShowMenu();
        }

        public void ShowPlayUI()
        {
            _state.ShowPlaying();
        }

        public void UpdateScoreText(int i)
        {
            score.UpdateScore(i);
        }

        public void UpdateHighScoreText(int highScore)
        {
            this.highScore.SetScore(highScore);
        }

        internal void FadeInNewHighScore()
        {
            highScore.FadeInNewHighScore(1f);
        }
    }
}