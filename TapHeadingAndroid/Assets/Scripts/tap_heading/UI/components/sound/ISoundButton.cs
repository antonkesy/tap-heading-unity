using tap_heading.UI.utility;
using tap_heading.UI.utility.Transition.Fade;

namespace tap_heading.UI.components.sound
{
    public interface ISoundButton : IFader
    {
        void Toggle();
    }
}