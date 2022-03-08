using System.Collections;
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
        [SerializeField] private float menuOffsetTime;

        public override void OnEntering()
        {
            about.Close();
            gameTitle.In();
            StartCoroutine(ShowMenu());
        }

        private IEnumerator ShowMenu()
        {
            yield return new WaitForSecondsRealtime(menuOffsetTime);
            tapToStartText.SetText("TAP TO START");
            menuManager.In();
            managers.GetGameManager().ReadyToStartGameCallback();
            yield return null;
        }

        public override void OnLeaving()
        {
            menuManager.Out();
            gameTitle.Out();
        }
    }
}