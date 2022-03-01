using UnityEngine;

namespace tap_heading.Game
{
    public interface IGameManager
    {
        void CoinPickedUpCallback();
        void DestroyPlayerCallback();
        void ReadyToStartGameCallback();
        void SetSingleClick(bool isSingle);
        void PlayerChangeDirection(Vector2 clickPosition);
        public bool IsClickForGame();
        public void Restart();
    }
}