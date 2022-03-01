using tap_heading.Audio;
using tap_heading.Camera;
using tap_heading.Game;
using tap_heading.Game.chunk;
using tap_heading.Game.level;
using tap_heading.Player;
using tap_heading.Services;
using tap_heading.Settings;
using tap_heading.UI;

namespace tap_heading.manager
{
    public interface IManagerCollector
    {
        IAudioManager GetAudioManager();
        ICameraManager GetCameraManager();
        IChunkManager GetChunkManager();
        ILevelManager GetLevelManager();
        IPlayerManager GetPlayerManager();
        IGameManager GetGameManager();
        IReviewService GetReviewService();
        IScoreService GetScoreService();
        UIManager GetUIManager();
        ISettings GetSettings();
    }
}