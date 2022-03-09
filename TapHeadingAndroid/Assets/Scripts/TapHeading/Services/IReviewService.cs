using UnityEngine;

namespace TapHeading.Services
{
    public interface IReviewService
    {
        public void RequestReview(MonoBehaviour monoBehaviour, int timesOpen);
    }
}