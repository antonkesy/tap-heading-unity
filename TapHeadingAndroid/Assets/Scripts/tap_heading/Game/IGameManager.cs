namespace tap_heading.Game
{
    public interface IGameManager
    {
        void CoinPickedUpCallback();
        void DestroyPlayerCallback();
        void ReadyToStartGameCallback();
        void SetSingleClick(bool isSingleClick);
    }
}