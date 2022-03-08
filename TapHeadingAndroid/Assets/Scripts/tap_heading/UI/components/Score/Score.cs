using tap_heading.UI.components.Text;
using tap_heading.UI.utility.Transition;
using tap_heading.UI.utility.Transition.Zoom;
using TMPro;
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

        public void ShowPlaying()
        {
            _active?.Out();
            _active = playingZoom;
            _active.In();
        }

        public void ShowMenu()
        {
            _active?.Out();
            _active = menuZoom;
            _active.In();
        }

        public void UpdateScore(int score)
        {
            playing.SetText(score.ToString());
            menu.SetText(score.ToString());
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