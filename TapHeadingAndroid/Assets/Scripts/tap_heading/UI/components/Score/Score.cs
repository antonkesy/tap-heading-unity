using System;
using tap_heading.UI.components.Text;
using tap_heading.UI.utility;
using tap_heading.UI.utility.Transition;
using tap_heading.UI.utility.Transition.Fade;
using UnityEngine;

namespace tap_heading.UI.components.Score
{
    public class Score : MonoBehaviour, ITransition
    {
        [SerializeField] private ShadowText playing;
        [SerializeField] private ShadowText menu;

        private ITransition _active;

        private void Awake()
        {
            menu.SetActive(false);
            playing.SetActive(false);
        }

        public void ShowPlaying()
        {
            menu.SetActive(false);
            playing.SetActive(true);
            _active = playing;
        }

        public void ShowMenu()
        {
            menu.SetActive(true);
            playing.SetActive(false);
            _active = menu;
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