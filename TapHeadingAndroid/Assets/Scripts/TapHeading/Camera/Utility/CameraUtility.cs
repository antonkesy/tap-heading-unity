using UnityEngine;

namespace TapHeading.Camera.Utility
{
    public class CameraUtility
    {
        public static float GetFrustumHeight()
        {
            var mainCam = UnityEngine.Camera.main;
            if (mainCam is null) return 0f;
            var frustumHeight = 2.0f * mainCam.orthographicSize *
                                Mathf.Tan(mainCam.fieldOfView * 0.5f * Mathf.Deg2Rad);

            return frustumHeight;
        }
    }
}