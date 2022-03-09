using TapHeading.Audio;
using TapHeading.Camera;
using TapHeading.Game;
using TapHeading.Game.Level;
using TapHeading.Game.Level.Obstacle;
using TapHeading.Player;
using TapHeading.Services;
using TapHeading.Settings;
using TapHeading.UI;

namespace TapHeading.Manager
{
    public interface IManagerCollector
    {
        IAudioManager GetAudioManager();
        ICameraManager GetCameraManager();
        IObstacle GetChunkManager();
        ILevelManager GetLevelManager();
        IPlayerManager GetPlayerManager();
        IGameManager GetGameManager();
        IReviewService GetReviewService();
        IScoreService GetScoreService();
        UIManager GetUIManager();
        ISettings GetSettings();
    }
}