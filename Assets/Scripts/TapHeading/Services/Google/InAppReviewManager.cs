using System.Collections;
using Google.Play.Review;
using UnityEngine;

namespace TapHeading.Services.Google
{
    public class InAppReviewManager : IReviewService
    {
        private static IReviewService _instance;
        private ReviewManager _reviewManager;

        private InAppReviewManager() { }

        public static IReviewService Instance => _instance ??= new InAppReviewManager();

        private IEnumerator RequestReviewFlow()
        {
            //built on first use so Play Core is not touched for players who never reach 30 opens
            _reviewManager ??= new ReviewManager();
            var requestFlowOperation = _reviewManager.RequestReviewFlow();
            yield return requestFlowOperation;
            if (requestFlowOperation.Error != ReviewErrorCode.NoError)
            {
                yield break;
            }

            //nothing to do about a failed launch, the store decides whether to show anything
            yield return _reviewManager.LaunchReviewFlow(requestFlowOperation.GetResult());
        }

        public void RequestReview(MonoBehaviour monoBehaviour, int timesOpen)
        {
            if (timesOpen < 30)
                return;
            monoBehaviour.StartCoroutine(RequestReviewFlow());
        }
    }
}
