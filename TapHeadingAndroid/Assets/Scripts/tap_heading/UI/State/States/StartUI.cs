using tap_heading.UI.components.About;
using tap_heading.UI.components.Text;
using tap_heading.UI.components.Title;
using UnityEngine;
using UnityEngine.UI;

namespace tap_heading.UI.State.States
{
    public class StartUI : UIState
    {
        [SerializeField] protected AboutUI about;
        [SerializeField] protected UIMenuManager menuManager;
        [SerializeField] protected ShadowText tapToStartText;
        [SerializeField] protected GameTitle gameTitle;

        public override void OnEntering()
        {
            score.HideAll();
            about.Close();
            score.FadeOut(0f);
            menuManager.FadeOut(0f);
            gameTitle.SlideIn();
            //wait 1sec 
            menuManager.FadeIn(.5f);
            tapToStartText.SetText("TAP TO START");
            managers.GetGameManager().ReadyToStartGameCallback();
        }

        public override void OnLeaving()
        {
            menuManager.FadeOut(0.5f);
            gameTitle.SlideOut();
        }
    }
}