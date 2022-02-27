/*
MIT License
Copyright (c) 2021 Anton Kesy
Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:
The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.
THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
 */

using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

/**
 * Manages the game loop as god class 
 */
public class GameManager : MonoBehaviour
{
    private static GameManager Instance { get; set; }

    [Header("Manager")] [SerializeField] private PlayerManager playerManager;
    [SerializeField] private LevelManager levelManager;
    [SerializeField] private UIManager uiManager;
    [SerializeField] private CameraManager cameraManager;

    [SerializeField] private AudioManager audioManager;
    private static int _highScore;

    //Score
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

    private void Awake()
    {
        GPSManager.Activate();
    }

    private void Start()
    {
        Instance = this;
        SetHighScoreLocal();
        LoadFlagsFromPlayerPrefs();
        Application.targetFrameRate = 60;
        uiManager.ShowStartMenuUI();
        audioManager.PlayStartApplication();
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
        _isIarPopUpPossible = PlayerPrefsManager.GetTimesOpen() < TimesToOpenB4IarCall &&
                              PlayerPrefsManager.GetTimesPlayed() < TimesToPlayB4IarCall;

        if (PlayerPrefsManager.IsAutoLogin())
        {
            GPSManager.SignInToGooglePlayServices();
        }

        audioManager.SetSound(PlayerPrefsManager.IsSoundOn());
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
        PlayerPrefsManager.SetLocalHighScore(_highScore);
    }

    /**
     * Sets local highScore
     */
    private void SetHighScoreLocal()
    {
        SetHighScore(PlayerPrefsManager.GetLocalHighScore());
    }

    // ReSharper disable once InconsistentNaming
    /**
     * Overwrites GPS highScore with local highScore 
     */
    internal static void OverwriteGPSHighScore()
    {
        GPSManager.SubmitScore(PlayerPrefsManager.GetLocalHighScore());
    }

    // ReSharper disable once InconsistentNaming
    /**
     * Sets LocalHighScore
     */
    internal static void SetHighScoreFromGPS(long highScore)
    {
        Instance.SetHighScore((int)highScore);
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
        bool changedDirection = isSingleClick
            ? playerManager.ChangeDirection()
            : position.x > Screen.width / 2
                ? playerManager.CallChangeDirectionRight()
                : playerManager.CallChangeDirectionLeft();

        //play click audio if changed direction
        if (changedDirection)
        {
            audioManager.PlayTapPlayer();
        }
    }

    /**
     * Returns if the touch click should be processed as game tap
     */
    private bool IsClickForGame()
    {
        if (uiManager.isAboutOn()) return false;
        audioManager.PlayTapPlayer();
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
        levelManager.RestartLevel();
    }


    /**
     * Starts Fresh Game
     */
    private void StartFreshGame()
    {
        _currentGameState = GameState.Running;
        levelManager.StartFreshLevel();
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
        audioManager.PlayCollectCoin();
        uiManager.UpdateScoreText(++_score);
        levelManager.AddSpeed();
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
        audioManager.PlayDestroyPlayer();
        cameraManager.StartShaking();
        _currentGameState = GameState.Waiting;
        uiManager.ShowReturningMenuUI();
        StartCoroutine(WaitToRestart());
        CheckNewHighScore();
        GPSManager.SubmitScore(_score);
        GPSManager.CheckAchievement(_score);
        levelManager.EndLevel();
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
        audioManager.PlayNewHighScore();
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
        if (PlayerPrefsManager.GetTimesPlayed() > TimesToPlayB4IarCall ||
            PlayerPrefsManager.GetTimesOpen() > TimesToOpenB4IarCall)
        {
            StartCoroutine(IAReviewManager.RequestReview());
        }

        PlayerPrefsManager.AddTimesOpen();
        PlayerPrefsManager.AddTimesPlayed();
    }
}