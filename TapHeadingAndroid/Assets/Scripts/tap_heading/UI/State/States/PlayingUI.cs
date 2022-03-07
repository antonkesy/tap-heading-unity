namespace tap_heading.UI.State.States
{
    public class PlayingUI : UIState
    {
        public override void OnEntering()
        {
            score.ShowPlaying();
            score.FadeIn(.35f);
        }

        public override void OnLeaving()
        {
            //TODO
        }
    }
}