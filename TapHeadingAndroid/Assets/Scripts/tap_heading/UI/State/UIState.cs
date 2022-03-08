using System.Collections;
using tap_heading.manager;
using tap_heading.UI.components.highscore;
using UnityEngine;

namespace tap_heading.UI.State
{
    public abstract class UIState : MonoBehaviour
    {
        [SerializeField] protected ManagerCollector managers;
        [SerializeField] protected components.Score.Score score;
        [SerializeField] protected HighScoreUI highScoreUI;

        [SerializeField] protected float animationTime;

        protected abstract void OnEntering();
        protected abstract void OnLeaving();
        protected abstract void OnWaitAnimationDone();

        public void Enter()
        {
            OnEntering();
            StartCoroutine(WaitAnimation());
        }


        public void Leave()
        {
            OnLeaving();
        }

        private IEnumerator WaitAnimation()
        {
            yield return new WaitForSecondsRealtime(animationTime);
            OnWaitAnimationDone();
            yield return null;
        }

        internal void UpdateScoreText(int newScore)
        {
            score.UpdateScore(newScore);
        }

        internal void UpdateHighScoreText(int newScore)
        {
            highScoreUI.SetScore(newScore);
        }

        internal void FadeInNewHighScore()
        {
            //menuManager.FadeInNewHighScore(1f);
        }
    }
}