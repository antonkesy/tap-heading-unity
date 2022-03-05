using tap_heading.Audio;
using tap_heading.Camera;
using tap_heading.Game;
using tap_heading.Game.level;
using tap_heading.Game.level.obstacle;
using tap_heading.Player;
using tap_heading.Services;
using tap_heading.Services.Google;
using tap_heading.Settings;
using tap_heading.UI;
using UnityEngine;

namespace tap_heading.manager
{
    public class ManagerCollector : MonoBehaviour, IManagerCollector
    {
        [SerializeField] private AudioManager audioManager;
        [SerializeField] private CameraManager cameraManager;
        [SerializeField] private ObstacleManager obstacleManager;
        [SerializeField] private LevelManager levelManager;
        [SerializeField] private PlayerManager playerManager;
        [SerializeField] private GameManager gameManager;
        [SerializeField] private UIManager uiManager;
        [SerializeField] IAReviewManager inAppReviewService;
        private GooglePlayServicesManager _googleServiceManager;
        private ISettings _settings;

        private void Awake()
        {
            _googleServiceManager = GooglePlayServicesManager.Instance;
            _settings = new PlayerPrefsManager();
        }

        public IAudioManager GetAudioManager()
        {
            return audioManager;
        }

        public ICameraManager GetCameraManager()
        {
            return cameraManager;
        }

        public IObstacleManager GetChunkManager()
        {
            return obstacleManager;
        }

        public ILevelManager GetLevelManager()
        {
            return levelManager;
        }

        public IPlayerManager GetPlayerManager()
        {
            return playerManager;
        }

        public IGameManager GetGameManager()
        {
            return gameManager;
        }

        public IReviewService GetReviewService()
        {
            return inAppReviewService;
        }

        public IScoreService GetScoreService()
        {
            return _googleServiceManager;
        }

        public UIManager GetUIManager()
        {
            return uiManager;
        }

        public ISettings GetSettings()
        {
            return _settings;
        }
    }
}