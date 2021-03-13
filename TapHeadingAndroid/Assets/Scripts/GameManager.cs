using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

public class GameManager : MonoBehaviour
{
    [FormerlySerializedAs("playerMovementScript")] [SerializeField]
    private PlayerManager playerManager;

    [FormerlySerializedAs("levelGeneratorScript")] [SerializeField]
    private LevelManager levelManager;

    [SerializeField] private UIManager uiManager;
    [SerializeField] private CameraManager cameraManager;

    private bool _isRunning;

    private bool _waitingToStartFreshGame = true;

    private bool _waitingToRestartGame = false;

    private int _score;

    private int _highScore;

    void Start()
    {
        Application.targetFrameRate = 60;
        playerManager.SetManager(this);
    }

    // Update is called once per frame
    private void Update()
    {
        //TODO("to touch")
        if (Input.GetMouseButtonDown(0))
            if (_isRunning)
            {
                playerManager.CallChangeDirection();
            }
            else if (_waitingToStartFreshGame)
            {
                StartGame();
            }
            else if (_waitingToRestartGame)
            {
                RestartGame();
            }
    }

    private void RestartGame()
    {
        _score = 0;
        uiManager.UpdateScoreText(_score);
        _isRunning = true;
        _waitingToRestartGame = false;
        levelManager.RestartGame();
        uiManager.ShowPlayUI();
        playerManager.Restart();
    }

    private void StartGame()
    {
        _isRunning = true;
        _waitingToStartFreshGame = false;
        levelManager.StartGame();
        uiManager.ShowPlayUI();
        playerManager.StartMoving();
    }

    internal void CoinPickedUpCallback()
    {
        //Todo()
        uiManager.UpdateScoreText(++_score);
        levelManager.AddSpeed();
    }

    internal void DestroyPlayerCallback()
    {
        OnPlayerDestroy();
    }

    private void OnPlayerDestroy()
    {
        //TODO()
        cameraManager.StartShaking();
        levelManager.Pause();
        _isRunning = false;
        uiManager.ShowReturningMenuUI();
        StartCoroutine(WaitToRestart());
        _highScore = _highScore < _score ? _score : _highScore;
    }

    private IEnumerator WaitToRestart()
    {
        yield return new WaitForSecondsRealtime(1f);
        _waitingToRestartGame = true;
    }

    public void OnPause()
    {
        levelManager.Pause();
        playerManager.SetPause();
        _isRunning = false;
    }

    public void OnResume()
    {
        levelManager.Resume();
        playerManager.SetResume();
        _isRunning = true;
    }
}