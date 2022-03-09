using tap_heading.Game.States;
using tap_heading.input;
using tap_heading.manager;
using tap_heading.Player;
using tap_heading.Score;
using tap_heading.Services.Google;
using UnityEngine;

namespace tap_heading.Game
{
    public class GameManager : MonoBehaviour, IGameManager, IPlayerInputListener, IScoreListener

    {
        [SerializeField] private ManagerCollector managers;

        private bool _isIarPopUpPossible;
        private const int TimesToOpenB4IarCall = 10;
        private const int TimesToPlayB4IarCall = 50;

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
            _isIarPopUpPossible = managers.GetSettings().GetTimesOpen() < TimesToOpenB4IarCall &&
                                  managers.GetSettings().GetTimesPlayed() < TimesToPlayB4IarCall;

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

            //play click audio if changed direction
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
            managers.GetCameraManager().StartShaking();
            _gameState = new WaitForAnimation();
            managers.GetUIManager().ShowMenu();
            managers.GetLevelManager().Stop();
            managers.GetPlayerManager().Spawn();
            CheckForIARPopUp();

            if (_score.IsHighScore())
            {
                managers.GetAudioManager().PlayNewHighScore();
                managers.GetUIManager().FadeInNewHighScore();
            }
        }

        private void CheckForIARPopUp()
        {
            if (!_isIarPopUpPossible) return;
            if (managers.GetSettings().GetTimesPlayed() > TimesToPlayB4IarCall ||
                managers.GetSettings().GetTimesOpen() > TimesToOpenB4IarCall)
            {
                IAReviewManager.Instance.RequestReview();
            }

            managers.GetSettings().IncrementTimesOpen();
            managers.GetSettings().IncrementTimesPlayed();
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