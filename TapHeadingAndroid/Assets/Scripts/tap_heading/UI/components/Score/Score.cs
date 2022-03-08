using tap_heading.UI.components.Text;
using tap_heading.UI.utility.Transition;
using tap_heading.UI.utility.Transition.Zoom;
using UnityEngine;

namespace tap_heading.UI.components.Score
{
    public class Score : MonoBehaviour, ITransition
    {
        [SerializeField] private ShadowText playing;
        [SerializeField] private ShadowText menu;
        [SerializeField] private ZoomUI playingZoom;
        [SerializeField] private ZoomUI menuZoom;

        private ITransition _active;
        private bool isForceReset = false;

        public void ShowPlaying()
        {
            _active?.Out();
            _active = playingZoom;
            playing.SetText(0.ToString());
            isForceReset = true;
            _active.In();
        }

        public void ShowMenu()
        {
            if (isForceReset)
            {
                menu.SetText(0.ToString());
            }

            _active?.Out();
            _active = menuZoom;
            _active.In();
        }

        public void UpdateScore(int score)
        {
            playing.SetText(score.ToString());
            menu.SetText(score.ToString());
            isForceReset = false;
        }

        public void In()
        {
            _active?.In();
        }

        public void Out()
        {
            _active?.Out();
        }
    }
}