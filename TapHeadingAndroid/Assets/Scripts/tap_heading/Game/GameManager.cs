using System.Collections;
using System.Linq;
using tap_heading.Audio;
using tap_heading.Camera;
using tap_heading.Game.level;
using tap_heading.Player;
using tap_heading.Services.Google;
using tap_heading.Settings;
using tap_heading.UI;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace tap_heading.Game
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }

        [Header("Manager")] [SerializeField] private PlayerManager playerManager;
        private ILevelManager _levelManager;
        [SerializeField] private UIManager uiManager;
        [SerializeField] private GameObject cameraManagerHolder;
        private ICameraManager _cameraManager;

        private IAudioManager _audioManager;
        private static int _highScore;

        //Score
        private int _score;

        private bool _isIarPopUpPossible;
        private const int TimesToOpenB4IarCall = 10;
        private const int TimesToPlayB4IarCall = 50;

        private GameState _currentGameState = GameState.Waiting;
        [SerializeField] internal bool isSingleClick = true;

        private ISettings _settings;

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
            //workaround getting reference of interface implementation
            _audioManager = gameObject.transform.parent.GetComponentInChildren<IAudioManager>();
            _cameraManager = cameraManagerHolder.GetComponent<ICameraManager>();
            _levelManager = gameObject.transform.parent.GetComponentInChildren<ILevelManager>();
            _settings = new PlayerPrefsManager();
            SetHighScoreLocal();
            LoadFlagsFromPlayerPrefs();
            Application.targetFrameRate = 60;
            uiManager.ShowStartMenuUI();
            _audioManager.PlayStartApplication();
            playerManager.SpawnPlayer();
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

        /**
     * Loads flags from the player prefs
     */
        private void LoadFlagsFromPlayerPrefs()
        {
            _isIarPopUpPossible = _settings.GetTimesOpen() < TimesToOpenB4IarCall &&
                                  _settings.GetTimesPlayed() < TimesToPlayB4IarCall;

            _audioManager.SetSound(_settings.IsSoundOn());
        }

        /**
     * Sets game high score as static call
     */
        public static void SetHighScoreFromLocal()
        {
            Instance.SetHighScoreLocal();
        }

        /**
     * Sets game highScore and calls ui/save
     */
        private void SetHighScore(int value)
        {
            _highScore = value;
            uiManager.UpdateHighScoreText(_highScore);
            _settings.SetLocalHighScore(_highScore);
        }

        /**
     * Sets local highScore
     */
        private void SetHighScoreLocal()
        {
            SetHighScore(_settings.GetLocalHighScore());
        }

        // ReSharper disable once InconsistentNaming
        /**
     * Overwrites GPS highScore with local highScore 
     */
        internal void OverwriteGPSHighScore()
        {
            GooglePlayServicesManager.Instance.SubmitScore(_settings.GetLocalHighScore());
        }

        // ReSharper disable once InconsistentNaming
        /**
     * Sets LocalHighScore
     */
        internal static void SetHighScoreFromGPS(long highScore)
        {
            Instance.SetHighScore((int) highScore);
        }

        /**
     * Debug Editor Input
     */
        private void ProcessEditorInput()
        {
            if (Input.GetKeyDown(KeyCode.Space)) OnUserClick(Vector2.zero);

            //without sound!!!
            if (Input.GetKeyDown(KeyCode.LeftArrow)) playerManager.CallChangeDirectionLeft();
            if (Input.GetKeyDown(KeyCode.RightArrow)) playerManager.CallChangeDirectionRight();
        }

        /**
     * Processes touch input
     */
        private void ProcessUserInput()
        {
            if (Input.touchCount <= 0 || Input.GetTouch(0).phase != TouchPhase.Began) return;
            if (Input.touches.Select(touch => touch.fingerId)
                .Any(id => EventSystem.current.IsPointerOverGameObject(id)))
                return;

            OnUserClick(Input.GetTouch(Input.touchCount - 1).position);
        }

        /**
     * Calls Actions by user click depending on game state
     * <param name="position"></param>
     */
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
            var changedDirection = isSingleClick
                ? playerManager.ChangeDirection()
                : position.x > Screen.width / 2.0f
                    ? playerManager.CallChangeDirectionRight()
                    : playerManager.CallChangeDirectionLeft();

            //play click audio if changed direction
            if (changedDirection)
            {
                _audioManager.PlayPlayerTap();
            }
        }

        /**
     * Returns if the touch click should be processed as game tap
     */
        private bool IsClickForGame()
        {
            if (uiManager.isAboutOn()) return false;
            _audioManager.PlayPlayerTap();
            return true;
        }

        /**
     * Sets currentGameState to waiting for fresh game
     */
        internal void ReadyToStartGameCallback()
        {
            _currentGameState = GameState.WaitingForFreshGame;
        }

        /**
     * Restarts Game
     */
        private void RestartGame()
        {
            _currentGameState = GameState.Running;
            StartGame();
            _levelManager.RestartLevel();
        }


        /**
     * Starts Fresh Game
     */
        private void StartFreshGame()
        {
            _currentGameState = GameState.Running;
            _levelManager.StartFreshLevel();
            StartGame();
        }

        /**
     * Starts Game
     */
        private void StartGame()
        {
            _score = 0;
            uiManager.UpdateScoreText(_score);
            _currentGameState = GameState.Running;
            uiManager.ShowPlayUI();
            playerManager.StartMoving();
        }

        /**
     * Callback when coin is picked up
     */
        internal void CoinPickedUpCallback()
        {
            _audioManager.PlayCollectCoin();
            uiManager.UpdateScoreText(++_score);
            _levelManager.IncreaseSpeed();
        }

        /**
    * Callback when player is destroyed    
    */
        internal void DestroyPlayerCallback()
        {
            OnPlayerDestroy();
        }

        /**
     * Ends game
     */
        private void OnPlayerDestroy()
        {
            _audioManager.PlayPlayerDeath();
            _cameraManager.StartShaking();
            _currentGameState = GameState.Waiting;
            uiManager.ShowReturningMenuUI();
            StartCoroutine(WaitToRestart());
            CheckNewHighScore();
            GooglePlayServicesManager.Instance.SubmitScore(_score);
            GooglePlayServicesManager.Instance.CheckAchievement(_score);
            _levelManager.EndLevel();
            playerManager.SpawnPlayer();
            CheckForIARPopUp();
        }

        /**
     * Checks if current score is new highScore
     */
        private void CheckNewHighScore()
        {
            if (_highScore >= _score) return;
            SetHighScore(_score);
            _audioManager.PlayNewHighScore();
            uiManager.FadeInNewHighScore();
        }

        /**
     * Waits fixed time then sets currentGameState to WaitingToRestart 
     */
        private IEnumerator WaitToRestart()
        {
            yield return new WaitForSecondsRealtime(1f);
            _currentGameState = GameState.WaitingToRestart;
        }

        // ReSharper disable once InconsistentNaming
        /**
     * Checks if IARPopUp should be shown
     */
        private void CheckForIARPopUp()
        {
            if (!_isIarPopUpPossible) return;
            if (_settings.GetTimesPlayed() > TimesToPlayB4IarCall ||
                _settings.GetTimesOpen() > TimesToOpenB4IarCall)
            {
                IAReviewManager.Instance.RequestReview();
            }

            _settings.IncrementTimesOpen();
            _settings.IncrementTimesPlayed();
        }
    }
}