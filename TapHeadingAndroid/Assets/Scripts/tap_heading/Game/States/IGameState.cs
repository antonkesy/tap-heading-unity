using UnityEngine;

namespace tap_heading.Game.States
{
    public interface IGameState
    {
        public void OnUserClick(IGameManager manager, Vector2 clickPosition);
    }

    public class Running : IGameState
    {
        public void OnUserClick(IGameManager manager, Vector2 clickPosition)
        {
            manager.PlayerChangeDirection(clickPosition);
        }
    }

    public class WaitingRestart : IGameState
    {
        public void OnUserClick(IGameManager manager, Vector2 clickPosition)
        {
            if (manager.IsClickForGame()) manager.Restart();
        }
    }

    public class WaitForAnimation : IGameState
    {
        public void OnUserClick(IGameManager manager, Vector2 clickPosition)
        {
            //nothing
        }
    }
}