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

    private bool _waitingToStartFreshGame;

    private bool _waitingToRestartGame;

    private int _score;

    private int _highScore;

    private AudioSource _audioSource;
    [SerializeField] private AudioClip collectCoinAudioClip;
    [SerializeField] private AudioClip destroyPlayerAudioClip;

    private void Start()
    {
        Application.targetFrameRate = 60;
        playerManager.SetManager(this);
        uiManager.ShowStartMenuUI();
        _audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    private void Update()
    {
        //DEBUG----------------------------------------------

        if (Input.GetKeyDown("space"))
        {
            if (_isRunning)
            {
                playerManager.CallChangeDirection();
            }
            else if (_waitingToStartFreshGame)
            {
                if (uiManager.isAboutOn()) return;
                StartGame();
            }
            else if (_waitingToRestartGame)
            {
                if (uiManager.isAboutOn()) return;
                RestartGame();
            }
        }

        //-------------------------------------------------

        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
            // if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began || Input.GetButtonDown("Fire1"))
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
                if (uiManager.isAboutOn()) return;
                StartGame();
            }
            else if (_waitingToRestartGame)
            {
                if (uiManager.isAboutOn()) return;
                RestartGame();
            }
        }
    }

    internal void ReadyToStartGameCallback()
    {
        _waitingToStartFreshGame = true;
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
        _audioSource.PlayOneShot(collectCoinAudioClip, 1f);
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
        _audioSource.PlayOneShot(destroyPlayerAudioClip, 1f);
        cameraManager.StartShaking();
        levelManager.Pause();
        _isRunning = false;
        uiManager.ShowReturningMenuUI();
        StartCoroutine(WaitToRestart());

        if (_highScore < _score)
        {
            _highScore = _score;
            uiManager.UpdateHighScoreText(_highScore);
            PlayerPrefs.SetInt("highScore", _highScore);
        }
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
        levelManager.ResetGame();
        playerManager.ResetPlayer();
        _waitingToRestartGame = true;
    }

    public void SetSound(bool soundOn)
    {
        //throw new System.NotImplementedException();
    }
}