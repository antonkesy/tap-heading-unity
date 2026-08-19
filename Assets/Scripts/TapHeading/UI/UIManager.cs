using TapHeading.Manager;
using TapHeading.UI.Components.About;
using TapHeading.UI.Components.HighScore;
using TapHeading.UI.State;
using TapHeading.UI.State.States;
using UnityEngine;
using UnityEngine.UI;

namespace TapHeading.UI
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField]
        private ManagerCollector managers;

        [SerializeField]
        private AboutUI aboutPanel;

        [SerializeField]
        private Components.Score.Score score;

        [SerializeField]
        private HighScoreUI highScore;

        [SerializeField]
        private Toggle inputToggle;

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
            RestoreInputToggle();
        }

        /// <summary>
        /// Start, not Awake: ManagerCollector builds its settings in its own Awake.
        /// </summary>
        private void RestoreInputToggle()
        {
            if (inputToggle == null || managers == null)
            {
                Debug.LogError("UIManager: inputToggle/managers not assigned in the scene", this);
                return;
            }

            //without notify, so restoring the saved value does not fire ToggleInputSettings
            inputToggle.SetIsOnWithoutNotify(managers.GetSettings().IsSingleClick());
        }

        internal bool CancelAbout()
        {
            if (!aboutPanel.IsOpen())
                return false;

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

        /// <summary>
        /// Wired to the InputToggle in GameScene, which passes its own value.
        /// Must stay idempotent: Toggle.Rebuild re-invokes onValueChanged from the editor's
        /// canvas update, including outside play mode where nothing is initialised yet.
        /// </summary>
        public void ToggleInputSettings(bool isSingleClick)
        {
            if (!Application.isPlaying)
                return;
            if (managers.GetSettings().IsSingleClick() == isSingleClick)
                return;

            managers.GetSettings().SetSingleClick(isSingleClick);
            managers.GetGameManager().SetSingleClick(isSingleClick);
            managers.GetAudioManager().PlayUITap();
        }

        public void UpdateScoreText(int newScore)
        {
            score.UpdateScore(newScore);
        }

        public void UpdateHighScoreText(int highScoreValue)
        {
            highScore.SetScore(highScoreValue);
        }

        internal void FadeInNewHighScore()
        {
            highScore.FadeInNewHighScore();
        }
    }
}
