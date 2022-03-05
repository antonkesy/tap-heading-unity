namespace tap_heading.UI.components.Title
{
    public interface IGameTitle
    {
        public void SlideIn(IGameTitleListener listener);
    }

    public interface IGameTitleListener
    {
        void OnSlideInDone();
    }
}