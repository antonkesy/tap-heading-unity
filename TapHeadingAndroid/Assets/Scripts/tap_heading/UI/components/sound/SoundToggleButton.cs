using tap_heading.manager;
using tap_heading.UI.utility;
using UnityEngine;

namespace tap_heading.UI
{
    public class SoundToggleButton : MonoBehaviour, ISoundButton
    {
        [SerializeField] private UIFader soundOnFader;
        [SerializeField] private UIFader soundOffFader;

        [SerializeField] private ManagerCollector managers;

        private void Start()
        {
            managers.GetSettings().IsSoundOn();
        }

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
            soundOffFader.gameObject.SetActive(!managers.GetSettings().IsSoundOn());
            soundOnFader.gameObject.SetActive(managers.GetSettings().IsSoundOn());
        }
    }
}