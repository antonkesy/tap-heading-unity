using TapHeading.Audio;
using TapHeading.Camera;
using TapHeading.Game;
using TapHeading.Game.Level;
using TapHeading.Player;
using TapHeading.Settings;
using TapHeading.UI;

namespace TapHeading.Manager
{
    public interface IManagerCollector
    {
        IAudioManager GetAudioManager();
        ICameraShake GetCameraShaker();
        ILevelManager GetLevelManager();
        IPlayerManager GetPlayerManager();
        IGameManager GetGameManager();
        UIManager GetUIManager();
        ISettings GetSettings();
    }
}
