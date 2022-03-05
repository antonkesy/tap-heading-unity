using tap_heading.Settings;
using tap_heading.UI.utility;
using UnityEngine;

namespace tap_heading.UI.components.sound
{
    public class SoundToggleButton : MonoBehaviour, ISoundButton
    {
        [SerializeField] private UIFader soundOnFader;
        [SerializeField] private UIFader soundOffFader;

        [SerializeField] private PlayerPrefsManager settings;

        public void FadeIn(float duration)
        {
            soundOffFader.FadeIn(duration);
            soundOnFader.FadeIn(duration);
        }

        public void FadeOut(float duration)
        {
            soundOffFader.FadeOut(duration);
            soundOnFader.FadeOut(duration);
        }

        public void Toggle()
        {
            soundOffFader.gameObject.SetActive(!settings.IsSoundOn());
            soundOnFader.gameObject.SetActive(settings.IsSoundOn());
        }
    }
}