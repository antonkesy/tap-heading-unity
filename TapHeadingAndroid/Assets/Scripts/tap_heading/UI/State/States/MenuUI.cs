using tap_heading.UI.components.Title;
using TMPro;
using UnityEngine;

namespace tap_heading.UI.State.States
{
    public class MenuUI : UIState
    {
        [SerializeField] protected UIMenuManager menuManager;
        [SerializeField] protected GameObject aboutPanel;
        [SerializeField] protected TextMeshProUGUI tapToStartText;
        [SerializeField] protected GameTitle gameTitle;

        public override void OnEntering()
        {
            score.ShowMenu();
            gameTitle.SlideIn();
            menuManager.FadeIn(.2f);
            tapToStartText.text = "TAP TO RESTART";
            managers.GetGameManager().ReadyToStartGameCallback();
        }

        public override void OnLeaving()
        {
            aboutPanel.SetActive(false);
            menuManager.FadeOut(0.2f);
            gameTitle.SlideOut();
        }
    }
}