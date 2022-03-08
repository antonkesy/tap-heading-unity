using tap_heading.UI.components.Text;
using tap_heading.UI.utility.Transition;
using UnityEngine;

namespace tap_heading.UI.components.Score
{
    public class Score : MonoBehaviour, ITransition
    {
        [SerializeField] private ShadowText playing;
        [SerializeField] private ShadowText menu;

        private ITransition _active;

        public void ShowPlaying()
        {
            _active?.Out();
            _active = playing;
            _active.In();
        }

        public void ShowMenu()
        {
            _active?.Out();
            _active = menu;
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