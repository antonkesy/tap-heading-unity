using TapHeading.UI.Components.About;
using TapHeading.UI.Components.Text;
using TapHeading.UI.Components.Title;
using UnityEngine;

namespace TapHeading.UI.State.States
{
    public class StartUI : UIState
    {
        [SerializeField] protected AboutUI about;
        [SerializeField] protected UIMenuManager menuManager;
        [SerializeField] protected TapInfo tapToStartText;
        [SerializeField] protected GameTitle gameTitle;

        protected override void OnEntering()
        {
            about.Close();
            gameTitle.In();
        }

        protected override void OnLeaving()
        {
            menuManager.Out();
            gameTitle.Out();
        }

        protected override void OnWaitAnimationDone()
        {
            tapToStartText.SetText("TAP TO START");
            menuManager.In();
            managers.GetGameManager().ReadyToStartGameCallback();
        }
    }
}