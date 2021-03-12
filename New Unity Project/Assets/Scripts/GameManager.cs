using UnityEngine;
using UnityEngine.Serialization;

public class GameManager : MonoBehaviour
{
    [FormerlySerializedAs("playerMovementScript")] [SerializeField]
    private PlayerManager playerManager;

    [SerializeField] private LevelGeneratorScript levelGeneratorScript;
    [SerializeField] private UIManager uiManager;
    [SerializeField] private CameraManager cameraManager;

    private bool _isRunning = false;

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
            else
            {
                _isRunning = true;
                levelGeneratorScript.StartGame();
            }
    }

    internal void CoinPickedUpCallback()
    {
        //Todo()
        uiManager.UpdateScoreText(++_score);
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
}