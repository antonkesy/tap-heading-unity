using TapHeading.UI.Components.About;
using TapHeading.UI.Components.Text;
using TapHeading.UI.Components.Title;
using UnityEngine;

namespace TapHeading.UI.State.States
{
    public class MenuUI : UIState
    {
        [SerializeField] protected UIMenuManager menuManager;
        [SerializeField] protected AboutUI about;
        [SerializeField] protected TapInfo tapToStartText;
        [SerializeField] protected GameTitle gameTitle;

        protected override void OnEntering()
        {
            score.ShowMenu();
            gameTitle.In();
            menuManager.In();
            tapToStartText.SetText("TAP TO RESTART");
        }

        protected override void OnWaitAnimationDone()
        {
            managers.GetGameManager().ReadyToStartGameCallback();
        }

        protected override void OnLeaving()
        {
            about.Close();
            menuManager.Out();
            gameTitle.Out();
        }
    }
}