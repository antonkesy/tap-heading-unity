using tap_heading.UI.components.Title;
using TMPro;
using UnityEngine;

namespace tap_heading.UI.State.States
{
    public class StartUI : UIState, IGameTitleListener
    {
        [SerializeField] protected GameObject aboutPanel;
        [SerializeField] protected UIMenuManager menuManager;
        [SerializeField] protected TextMeshProUGUI tapToStartText;
        [SerializeField] protected GameTitle gameTitle;

        public override void OnEntering()
        {
            score.HideAll();
            aboutPanel.SetActive(false);
            score.FadeOut(0f);
            menuManager.FadeOut(0f);
            gameTitle.SlideIn(this);
        }

        public override void OnLeaving()
        {
            menuManager.FadeOut(0.5f);
            gameTitle.FadeOut(0.5f);
        }

        public void OnSlideInDone()
        {
            menuManager.FadeIn(.5f);
            tapToStartText.text = "TAP TO START";
            managers.GetGameManager().ReadyToStartGameCallback();
        }
    }
}