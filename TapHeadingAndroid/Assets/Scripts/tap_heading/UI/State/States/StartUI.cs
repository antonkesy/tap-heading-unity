using tap_heading.UI.components.About;
using tap_heading.UI.components.Text;
using tap_heading.UI.components.Title;
using UnityEngine;

namespace tap_heading.UI.State.States
{
    public class StartUI : UIState
    {
        [SerializeField] protected AboutUI about;
        [SerializeField] protected UIMenuManager menuManager;
        [SerializeField] protected TapInfo tapToStartText;
        [SerializeField] protected GameTitle gameTitle;

        public override void OnEntering()
        {
            about.Close();
            gameTitle.In();
            //TODO wait 1sec 
            menuManager.In();
            tapToStartText.SetText("TAP TO START");
            managers.GetGameManager().ReadyToStartGameCallback();
        }

        public override void OnLeaving()
        {
            menuManager.Out();
            gameTitle.Out();
        }
    }
}