namespace tap_heading.UI.State.States
{
    public class PlayingUI : UIState
    {
        public override void OnEntering()
        {
            score.ShowPlaying();
            score.In();
        }

        public override void OnLeaving()
        {
            //TODO
        }
    }
}