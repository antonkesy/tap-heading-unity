using tap_heading.UI.utility;
using UnityEngine;

namespace tap_heading.UI
{
    public class SoundToggleButton : MonoBehaviour, ISoundButton
    {
        [Header("Sound Buttons")] [SerializeField]
        private UIFader soundOnFader;

        [SerializeField] private UIFader soundOffFader;

        public void FadeIn(float duration)
        {
            //TODO
        }

        public void FadeOut(float duration)
        {
            //TODO
        }

        public void Toggle()
        {
            //TODO
        }
    }
}