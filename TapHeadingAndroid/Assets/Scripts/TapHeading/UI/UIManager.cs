using TapHeading.UI.Components.About;
using TapHeading.UI.Components.HighScore;
using TapHeading.UI.State;
using TapHeading.UI.State.States;
using UnityEngine;

namespace TapHeading.UI
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField] private AboutUI aboutPanel;
        [SerializeField] private Components.Score.Score score;
        [SerializeField] private HighScoreUI highScore;

        private StateMachine _state;

        private void Awake()
        {
            var start = GetComponent<StartUI>();
            var menu = GetComponent<MenuUI>();
            var playing = GetComponent<PlayingUI>();
            _state = new StateMachine(start, menu, playing);
        }

        private void Start()
        {
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