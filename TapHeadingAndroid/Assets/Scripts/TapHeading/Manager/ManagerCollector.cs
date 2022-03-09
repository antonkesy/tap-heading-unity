using System;
using TapHeading.Audio;
using TapHeading.Camera;
using TapHeading.Game;
using TapHeading.Game.Level;
using TapHeading.Game.Level.Obstacle;
using TapHeading.Player;
using TapHeading.Services;
using TapHeading.Services.Google;
using TapHeading.Settings;
using TapHeading.UI;
using UnityEngine;

namespace TapHeading.Manager
{
    public class ManagerCollector : MonoBehaviour, IManagerCollector
    {
        [SerializeField] private AudioManager audioManager;
        [SerializeField] private CameraShake cameraShake;
        [SerializeField] private Obstacle obstacle;
        [SerializeField] private LevelManager levelManager;
        [SerializeField] private PlayerManager playerManager;
        [SerializeField] private GameManager gameManager;
        [SerializeField] private UIManager uiManager;
        private PlayerPrefsManager _settings;

        private void Awake()
        {
            _settings = new PlayerPrefsManager();
        }

        public IAudioManager GetAudioManager()
        {
            return audioManager;
        }

        public ICameraShake GetCameraShaker()
        {
            return cameraShake;
        }

        public IObstacle GetChunkManager()
        {
            return obstacle;
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