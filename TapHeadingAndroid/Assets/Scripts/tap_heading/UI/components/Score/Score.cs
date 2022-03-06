using tap_heading.UI.components.Text;
using tap_heading.UI.utility;
using UnityEngine;

namespace tap_heading.UI.components.Score
{
    public class Score : MonoBehaviour, IFader
    {
        [SerializeField] private ShadowText playing;
        [SerializeField] private ShadowText menu;

        private IFader _active;

        public void HideAll()
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

        public void FadeIn(float duration)
        {
            _active?.FadeIn(duration);
        }

        public void FadeOut(float duration)
        {
            _active?.FadeOut(duration);
        }
    }
}