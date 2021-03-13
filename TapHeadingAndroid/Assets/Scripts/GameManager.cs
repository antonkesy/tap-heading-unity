using UnityEngine;
using UnityEngine.Serialization;

public class GameManager : MonoBehaviour
{
    [FormerlySerializedAs("playerMovementScript")] [SerializeField]
    private PlayerManager playerManager;

    [FormerlySerializedAs("levelGeneratorScript")] [SerializeField] private LevelManager levelManager;
    [SerializeField] private UIManager uiManager;
    [SerializeField] private CameraManager cameraManager;

    private bool _isRunning;

    private bool _waitingToStartNewGame = true;

    private int _score;

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
            else if (_waitingToStartNewGame)
            {
                _isRunning = true;
                _waitingToStartNewGame = false;
                levelManager.StartGame();
                uiManager.ShowPlayUI();
            }
    }

    internal void CoinPickedUpCallback()
    {
        //Todo()
        uiManager.UpdateScoreText(++_score);
        levelManager.AddSpeed();
    }

    internal void WallCollisionCallback()
    {
        //TODO()
        cameraManager.StartShaking();
    }

    internal void BarCollisionCallback()
    {
        //TODO()
        cameraManager.StartShaking();
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