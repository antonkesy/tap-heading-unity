using System.Collections;
using System.Linq;
using tap_heading.manager;
using tap_heading.Player;
using tap_heading.Services.Google;
using UnityEngine;
using UnityEngine.EventSystems;

namespace tap_heading.Game
{
    public class GameManager : MonoBehaviour, IGameManager
    {
        [SerializeField] private ManagerCollector managers;
        public static GameManager Instance { get; private set; }

        private static int _highScore;

        private int _score;

        private bool _isIarPopUpPossible;
        private const int TimesToOpenB4IarCall = 10;
        private const int TimesToPlayB4IarCall = 50;

        private GameState _currentGameState = GameState.Waiting;
        [SerializeField] internal bool isSingleClick = true;

        private enum GameState
        {
            Running,
            WaitingForFreshGame,
            WaitingToRestart,
            Waiting
        }

        private void Start()
        {
            Instance = this;
            SetHighScoreLocal();
            LoadFlagsFromPlayerPrefs();
            Application.targetFrameRate = 60;
            managers.GetUIManager().ShowStartMenuUI();
            managers.GetAudioManager().PlayStartApplication();
            managers.GetPlayerManager().Spawn();
        }


        private void Update()
        {
            //DEBUG----------------------------------------------
#if UNITY_EDITOR
            ProcessEditorInput();
#endif
            //-------------------------------------------------
            ProcessUserInput();
        }

        private void LoadFlagsFromPlayerPrefs()
        {
            _isIarPopUpPossible = managers.GetSettings().GetTimesOpen() < TimesToOpenB4IarCall &&
                                  managers.GetSettings().GetTimesPlayed() < TimesToPlayB4IarCall;

            managers.GetAudioManager().SetSound(managers.GetSettings().IsSoundOn());
        }

        public static void SetHighScoreFromLocal()
        {
            Instance.SetHighScoreLocal();
        }

        private void SetHighScore(int value)
        {
            _highScore = value;
            managers.GetUIManager().UpdateHighScoreText(_highScore);
            managers.GetSettings().SetLocalHighScore(_highScore);
        }

        private void SetHighScoreLocal()
        {
            SetHighScore(managers.GetSettings().GetLocalHighScore());
        }

        internal void OverwriteGPSHighScore()
        {
            GooglePlayServicesManager.Instance.SubmitScore(managers.GetSettings().GetLocalHighScore());
        }

        internal static void SetHighScoreFromGPS(long highScore)
        {
            Instance.SetHighScore((int) highScore);
        }

        private void ProcessEditorInput()
        {
            if (Input.GetKeyDown(KeyCode.Space)) OnUserClick(Vector2.zero);

            if (Input.GetKeyDown(KeyCode.LeftArrow))
                managers.GetPlayerManager().ChangeDirection(IPlayerManager.Direction.Left);
            else if (Input.GetKeyDown(KeyCode.RightArrow))
                managers.GetPlayerManager().ChangeDirection(IPlayerManager.Direction.Right);
        }

        private void ProcessUserInput()
        {
            if (Input.touchCount <= 0 || Input.GetTouch(0).phase != TouchPhase.Began) return;
            if (Input.touches.Select(touch => touch.fingerId)
                .Any(id => EventSystem.current.IsPointerOverGameObject(id)))
                return;

            OnUserClick(Input.GetTouch(Input.touchCount - 1).position);
        }

        private void OnUserClick(Vector2 position)
        {
            switch (_currentGameState)
            {
                case GameState.Running:
                    UserInteractionWhilePlaying(position);
                    break;
                case GameState.WaitingToRestart:
                    if (IsClickForGame()) RestartGame();
                    break;
                case GameState.WaitingForFreshGame:
                    if (IsClickForGame()) StartFreshGame();
                    break;
            }
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

        private bool IsClickForGame()
        {
            if (managers.GetUIManager().isAboutOn()) return false;
            managers.GetAudioManager().PlayPlayerTap();
            return true;
        }

        public void ReadyToStartGameCallback()
        {
            _currentGameState = GameState.WaitingForFreshGame;
        }

        public void SetSingleClick(bool isSingleClick)
        {
            this.isSingleClick = isSingleClick;
        }

        private void RestartGame()
        {
            _currentGameState = GameState.Running;
            StartGame();
            managers.GetLevelManager().RestartLevel();
        }


        private void StartFreshGame()
        {
            _currentGameState = GameState.Running;
            managers.GetLevelManager().StartFreshLevel();
            StartGame();
        }

        private void StartGame()
        {
            _score = 0;
            var uiManager = managers.GetUIManager();
            uiManager.UpdateScoreText(_score);
            _currentGameState = GameState.Running;
            uiManager.ShowPlayUI();
            managers.GetPlayerManager().StartMoving();
        }

        public void CoinPickedUpCallback()
        {
            managers.GetAudioManager().PlayCollectCoin();
            managers.GetUIManager().UpdateScoreText(++_score);
            managers.GetLevelManager().IncreaseSpeed();
        }

        public void DestroyPlayerCallback()
        {
            OnPlayerDestroy();
        }

        private void OnPlayerDestroy()
        {
            managers.GetAudioManager().PlayPlayerDeath();
            managers.GetCameraManager().StartShaking();
            _currentGameState = GameState.Waiting;
            managers.GetUIManager().ShowReturningMenuUI();
            StartCoroutine(WaitToRestart());
            CheckNewHighScore();
            GooglePlayServicesManager.Instance.SubmitScore(_score);
            GooglePlayServicesManager.Instance.CheckAchievement(_score);
            managers.GetLevelManager().EndLevel();
            managers.GetPlayerManager().Spawn();
            CheckForIARPopUp();
        }

        private void CheckNewHighScore()
        {
            if (_highScore >= _score) return;
            SetHighScore(_score);
            managers.GetAudioManager().PlayNewHighScore();
            managers.GetUIManager().FadeInNewHighScore();
        }

        private IEnumerator WaitToRestart()
        {
            yield return new WaitForSecondsRealtime(1f);
            _currentGameState = GameState.WaitingToRestart;
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
    }
}