using System.Collections;
using TapHeading.Manager;
using TapHeading.UI.Components.HighScore;
using UnityEngine;

namespace TapHeading.UI.State
{
    public abstract class UIState : MonoBehaviour
    {
        [SerializeField]
        protected ManagerCollector managers;

        [SerializeField]
        protected Components.Score.Score score;

        [SerializeField]
        protected HighScoreUI highScoreUI;

        [SerializeField]
        protected float animationTime;
        private Coroutine _waitAnimation;

        protected abstract void OnEntering();
        protected abstract void OnLeaving();
        protected abstract void OnWaitAnimationDone();

        public void Enter()
        {
            OnEntering();
            _waitAnimation = StartCoroutine(WaitAnimation());
        }

        public void Leave()
        {
            //otherwise a stale OnWaitAnimationDone fires after the state is gone
            if (_waitAnimation != null)
            {
                StopCoroutine(_waitAnimation);
                _waitAnimation = null;
            }

            OnLeaving();
        }

        private IEnumerator WaitAnimation()
        {
            yield return new WaitForSecondsRealtime(animationTime);
            _waitAnimation = null;
            OnWaitAnimationDone();
        }
    }
}
