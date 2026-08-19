using System.Collections;
using TapHeading.Manager;
using TapHeading.UI.Components.HighScore;
using UnityEngine;

namespace TapHeading.UI.State
{
    public abstract class UIState : MonoBehaviour
    {
        [SerializeField] protected ManagerCollector managers;
        [SerializeField] protected Components.Score.Score score;
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
    }
}