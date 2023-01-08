using System.Collections;
using Google.Play.Review;
using UnityEngine;

namespace TapHeading.Services.Google
{
    public class InAppReviewManager : IReviewService
    {
        private static IReviewService _instance;
        private static ReviewManager _reviewManager;
        private static PlayReviewInfo _playReviewInfo;

        private InAppReviewManager()
        {
        }

        public static IReviewService Instance => _instance ??= new InAppReviewManager();

        private static IEnumerator _RequestReview()
        {
            _reviewManager = new ReviewManager();
            var requestFlowOperation = _reviewManager.RequestReviewFlow();
            yield return requestFlowOperation;
            if (requestFlowOperation.Error != ReviewErrorCode.NoError)
            {
                yield break;
            }

            _playReviewInfo = requestFlowOperation.GetResult();

            var launchFlowOperation = _reviewManager.LaunchReviewFlow(_playReviewInfo);
            yield return launchFlowOperation;
            _playReviewInfo = null; // Reset the object
            if (launchFlowOperation.Error != ReviewErrorCode.NoError)
            {
                //nothing
            }

            yield break;
        }

        public void RequestReview(MonoBehaviour monoBehaviour, int timesOpen)
        {
            if (timesOpen < 30) return;
            monoBehaviour.StartCoroutine(_RequestReview());
        }
    }
}