using System.Collections;
using Google.Play.Review;
using UnityEngine;

// ReSharper disable once InconsistentNaming
namespace tap_heading.Services.Google
{
    /**
 * InAppReviewManager for Google Play Core IAR
 */
    public class IAReviewManager : MonoBehaviour, IReviewService
    {
        private static IReviewService _instance;
        private static ReviewManager _reviewManager;
        private static PlayReviewInfo _playReviewInfo;

        public static IReviewService Instance => _instance ??= new IAReviewManager();

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
                yield break;
            }
        }

        public void RequestReview()
        {
            StartCoroutine(_RequestReview());
        }
    }
}