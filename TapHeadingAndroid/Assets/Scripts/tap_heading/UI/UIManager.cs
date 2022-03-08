using tap_heading.UI.State;
using tap_heading.UI.State.States;
using UnityEngine;

namespace tap_heading.UI
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField] private GameObject aboutPanel;

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
            var result = aboutPanel.activeSelf;
            aboutPanel.SetActive(false);
            return result;
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
            _state.UpdateScoreText(i);
        }

        public void UpdateHighScoreText(int highScore)
        {
            _state.UpdateHighScoreText(highScore);
        }
    }
}