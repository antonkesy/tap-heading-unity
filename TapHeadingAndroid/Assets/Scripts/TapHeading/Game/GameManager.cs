using TapHeading.Game.States;
using TapHeading.Input;
using TapHeading.Manager;
using TapHeading.Player;
using TapHeading.Score;
using TapHeading.Services.Google;
using UnityEngine;

namespace TapHeading.Game
{
    public class GameManager : MonoBehaviour, IGameManager, IPlayerInputListener, IScoreListener

    {
        [SerializeField] private ManagerCollector managers;

        [SerializeField] internal bool isSingleClick = true;

        private IGameState _gameState = new WaitForAnimation();

        private IScore _score;

        private void Start()
        {
            Application.targetFrameRate = 200;
            QualitySettings.vSyncCount = 0;

            _score = new Score.Score(this, managers.GetSettings());
            LoadFlagsFromPlayerPrefs();
            managers.GetAudioManager().PlayStartApplication();
            managers.GetPlayerManager().Spawn();

            var input = gameObject.AddComponent<TouchInput>();
            input.AddListener(this);
#if UNITY_EDITOR
            var inputDebug = gameObject.AddComponent<DebugEditorInput>();
            inputDebug.AddListener(this);
#endif
        }

        private void LoadFlagsFromPlayerPrefs()
        {
            managers.GetSettings().IncrementTimesOpen();
            managers.GetAudioManager().SetSound(managers.GetSettings().IsSoundOn());
        }


        private void UserInteractionWhilePlaying(Vector2 position)
        {
            var playerManager = managers.GetPlayerManager();
            var changedDirection = isSingleClick
                ? playerManager.ChangeDirection()
                : position.x > Screen.width / 2.0f
                    ? playerManager.ChangeDirection(IPlayerManager.Direction.Right)
                    : playerManager.ChangeDirection(IPlayerManager.Direction.Left);

            if (changedDirection)
            {
                managers.GetAudioManager().PlayPlayerTap();
            }
        }

        public void PlayerChangeDirection(Vector2 clickPosition)
        {
            UserInteractionWhilePlaying(clickPosition);
        }

        public bool IsClickForGame()
        {
            if (managers.GetUIManager().CancelAbout()) return false;
            managers.GetAudioManager().PlayPlayerTap();
            return true;
        }

        public void ReadyToStartGameCallback()
        {
            _gameState = new WaitingRestart();
        }

        public void SetSingleClick(bool isSingle)
        {
            isSingleClick = isSingle;
        }


        public void Restart()
        {
            _score.Reset();
            _gameState = new Running();
            managers.GetUIManager().ShowPlayUI();
            managers.GetPlayerManager().StartMoving();
            managers.GetLevelManager().Restart();
        }

        public void OnScoreUpdate(int score)
        {
            _gameState.OnScoreUpdate(managers, score);
        }

        public void CoinPickedUpCallback()
        {
            _score.Add(1);
        }

        public void DestroyPlayerCallback()
        {
            OnPlayerDestroy();
        }

        private void OnPlayerDestroy()
        {
            managers.GetAudioManager().PlayPlayerDeath();
            managers.GetCameraShaker().Shake();
            _gameState = new WaitForAnimation();
            managers.GetUIManager().ShowMenu();
            managers.GetLevelManager().Stop();
            managers.GetPlayerManager().Spawn();
            InAppReviewManager.Instance.RequestReview(this, managers.GetSettings().GetTimesOpen());

            if (_score.IsHighScore())
            {
                managers.GetAudioManager().PlayNewHighScore();
                managers.GetUIManager().FadeInNewHighScore();
            }
        }

        public void OnClick(Vector2 position)
        {
            _gameState.OnUserClick(managers, position);
        }

        public void OnNewHighScore(int highScore)
        {
            managers.GetUIManager().UpdateHighScoreText(highScore);
        }
    }
}