namespace TapHeading.UI.State
{
    public class StateMachine
    {
        private UIState _current;

        private readonly UIState _start;
        private readonly UIState _menu;
        private readonly UIState _playing;

        public StateMachine(UIState start, UIState menu, UIState playing)
        {
            _start = start;
            _menu = menu;
            _playing = playing;
        }

        private void SwitchState(UIState to)
        {
            if (_current != null)
            {
                _current.Leave();
            }

            _current = to;
            _current.Enter();
        }

        public void ShowStart()
        {
            SwitchState(_start);
        }

        public void ShowMenu()
        {
            SwitchState(_menu);
        }

        public void ShowPlaying()
        {
            SwitchState(_playing);
        }
    }
}