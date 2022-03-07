using tap_heading.UI.components.About;
using tap_heading.UI.components.Title;
using TMPro;
using UnityEngine;

namespace tap_heading.UI.State.States
{
    public class StartUI : UIState
    {
        [SerializeField] protected AboutUI about;
        [SerializeField] protected UIMenuManager menuManager;
        [SerializeField] protected TextMeshProUGUI tapToStartText;
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
            tapToStartText.text = "TAP TO START";
            managers.GetGameManager().ReadyToStartGameCallback();
        }

        public override void OnLeaving()
        {
            menuManager.FadeOut(0.5f);
            gameTitle.SlideOut();
        }
    }
}