using UnityEngine;

namespace TapHeading.Camera.Utility
{
    public static class CameraUtility
    {
        /// <summary>
        /// Off-screen margin used to spawn and de-spawn things, in world units.
        /// NOT a frustum height: the camera is orthographic, so its real height is
        /// 2 * orthographicSize. This applies the perspective formula instead and lands on
        /// ~1.15 * orthographicSize, which is what the level offsets are tuned around.
        /// Changing the maths means re-tuning every spawn offset by eye.
        /// </summary>
        public static float GetSpawnHeight()
        {
            var mainCam = UnityEngine.Camera.main;
            if (mainCam is null)
                return 0f;

            return 2.0f
                * mainCam.orthographicSize
                * Mathf.Tan(mainCam.fieldOfView * 0.5f * Mathf.Deg2Rad);
        }
    }
}
