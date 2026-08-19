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
        ICameraShake GetCameraShaker();
        IObstacle GetChunkManager();
        ILevelManager GetLevelManager();
        IPlayerManager GetPlayerManager();
        IGameManager GetGameManager();
        UIManager GetUIManager();
        ISettings GetSettings();
    }
}