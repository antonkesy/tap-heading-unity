using TapHeading.UI.Components.Text;
using TapHeading.UI.Utility.Transition;
using TapHeading.UI.Utility.Transition.Zoom;
using UnityEngine;

namespace TapHeading.UI.Components.Score
{
    public class Score : MonoBehaviour, ITransition
    {
        [SerializeField] private ShadowText playing;
        [SerializeField] private ShadowText menu;
        [SerializeField] private ZoomUI playingZoom;
        [SerializeField] private ZoomUI menuZoom;

        private ITransition _active;
        private bool _isForceReset = false;

        public void ShowPlaying()
        {
            _active?.Out();
            _active = playingZoom;
            playing.SetText(0.ToString());
            _isForceReset = true;
            _active.In();
        }

        public void ShowMenu()
        {
            if (_isForceReset)
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
            _isForceReset = false;
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