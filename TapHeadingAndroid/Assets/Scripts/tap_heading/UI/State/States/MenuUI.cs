using tap_heading.UI.components.About;
using tap_heading.UI.components.Text;
using tap_heading.UI.components.Title;
using UnityEngine;

namespace tap_heading.UI.State.States
{
    public class MenuUI : UIState
    {
        [SerializeField] protected UIMenuManager menuManager;
        [SerializeField] protected AboutUI about;
        [SerializeField] protected ShadowText tapToStartText;
        [SerializeField] protected GameTitle gameTitle;

        public override void OnEntering()
        {
            score.ShowMenu();
            gameTitle.In();
            menuManager.In();
            tapToStartText.SetText("TAP TO RESTART");
            managers.GetGameManager().ReadyToStartGameCallback();
        }

        public override void OnLeaving()
        {
            about.Close();
            menuManager.Out();
            gameTitle.Out();
        }
    }
}