using System.Collections;
using Google.Play.Review;
using UnityEngine;

// ReSharper disable once InconsistentNaming
/**
 * InAppReviewManager for Google Play Core IAR
 */
public class IAReviewManager : MonoBehaviour
{
    private static ReviewManager _reviewManager;
    private static PlayReviewInfo _playReviewInfo;

    /**
     * Requests new Review and if no error shows banner
     */
    internal static IEnumerator RequestReview()
    {
        _reviewManager = new ReviewManager();
        var requestFlowOperation = _reviewManager.RequestReviewFlow();
        yield return requestFlowOperation;
        if (requestFlowOperation.Error != ReviewErrorCode.NoError)
        {
            // Log error. For example, using requestFlowOperation.Error.ToString().
            yield break;
        }

        _playReviewInfo = requestFlowOperation.GetResult();

        var launchFlowOperation = _reviewManager.LaunchReviewFlow(_playReviewInfo);
        yield return launchFlowOperation;
        _playReviewInfo = null; // Reset the object
        if (launchFlowOperation.Error != ReviewErrorCode.NoError)
        {
            // Log error. For example, using requestFlowOperation.Error.ToString().
            yield break;
        }
    }
}