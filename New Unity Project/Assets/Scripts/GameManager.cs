using UnityEngine;
using UnityEngine.Serialization;

public class GameManager : MonoBehaviour
{
    [FormerlySerializedAs("playerMovementScript")] [SerializeField]
    private PlayerManager playerManager;

    [SerializeField] private LevelGeneratorScript levelGeneratorScript;
    [SerializeField] private UIManager uiManager;
    [SerializeField] private CameraManager cameraManager;

    private bool _isRunning;

    private bool _waitingToStartNewGame = true;

    private int _score;

    void Start()
    {
        playerManager.SetManager(this);
    }

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
            if (_isRunning)
            {
                playerManager.CallChangeDirection();
            }
            else if (_waitingToStartNewGame)
            {
                _isRunning = true;
                _waitingToStartNewGame = false;
                levelGeneratorScript.StartGame();
            }
    }

    internal void CoinPickedUpCallback()
    {
        //Todo()
        uiManager.UpdateScoreText(++_score);
        levelGeneratorScript.AddSpeed();
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
        levelGeneratorScript.Pause();
        playerManager.SetPause();
        _isRunning = false;
    }

    public void OnResume()
    {
        levelGeneratorScript.Resume();
        playerManager.SetResume();
        _isRunning = true;
    }
}