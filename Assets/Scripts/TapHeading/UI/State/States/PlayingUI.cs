namespace TapHeading.UI.State.States
{
    public class PlayingUI : UIState
    {
        protected override void OnEntering()
        {
            score.ShowPlaying();
            score.In();
        }

        protected override void OnLeaving()
        {
            //nothing
        }

        protected override void OnWaitAnimationDone()
        {
            //nothing
        }
    }
}
