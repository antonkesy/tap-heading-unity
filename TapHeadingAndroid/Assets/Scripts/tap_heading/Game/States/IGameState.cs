using tap_heading.manager;
using UnityEngine;

namespace tap_heading.Game.States
{
    public interface IGameState
    {
        public void OnUserClick(IManagerCollector manager, Vector2 clickPosition);
        public void OnScoreUpdate(IManagerCollector managers, int score);
    }

    public class Running : IGameState
    {
        public void OnUserClick(IManagerCollector manager, Vector2 clickPosition)
        {
            manager.GetGameManager().PlayerChangeDirection(clickPosition);
        }

        public void OnScoreUpdate(IManagerCollector managers, int score)
        {
            managers.GetAudioManager().PlayCollectCoin();
            managers.GetUIManager().UpdateScoreText(score);
            managers.GetLevelManager().IncreaseSpeed();
        }
    }

    public class WaitingRestart : IGameState
    {
        public void OnUserClick(IManagerCollector manager, Vector2 clickPosition)
        {
            if (manager.GetGameManager().IsClickForGame()) manager.GetGameManager().Restart();
        }

        public void OnScoreUpdate(IManagerCollector managers, int score)
        {
            //nothing
        }
    }

    public class WaitForAnimation : IGameState
    {
        public void OnUserClick(IManagerCollector manager, Vector2 clickPosition)
        {
            //nothing
        }

        public void OnScoreUpdate(IManagerCollector managers, int score)
        {
            //nothing
        }
    }
}