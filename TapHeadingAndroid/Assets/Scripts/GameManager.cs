using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
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
        uiManager.ShowStartMenuUI();
    }

    // Update is called once per frame
    private void Update()
    {
        // if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began || Input.GetButtonDown("Fire1"))
        {
            if (Input.touches.Select(touch => touch.fingerId)
                .Any(id => EventSystem.current.IsPointerOverGameObject(id)))
            {
                return;
            }

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
        _score = 0;
        uiManager.UpdateScoreText(_score);
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

    public void ResetToStart()
    {
        //Todo();
    }
}