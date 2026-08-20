using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace TapHeading.Input
{
    public class TouchInput : UserInput
    {
        private static readonly List<RaycastResult> Hits = new List<RaycastResult>();

        protected override void ProcessInput()
        {
            if (UnityEngine.Input.touchCount <= 0)
                return;

            var touch = UnityEngine.Input.GetTouch(0);
            if (touch.phase != TouchPhase.Began)
                return;

            if (IsOverInteractableUI(touch.position))
                return;

            Notify(touch.position);
        }

        /// <summary>
        /// Raycasts instead of asking the EventSystem for its selection: the selection is only
        /// updated once the input module runs, which may be after this Update on the frame the
        /// touch begins. Selectable, not Button, so Toggles count too - and every text and
        /// background here is a raycast target, so a plain "over any UI" check would swallow
        /// gameplay taps.
        /// </summary>
        private static bool IsOverInteractableUI(Vector2 position)
        {
            if (EventSystem.current == null)
                return false;

            Hits.Clear();
            EventSystem.current.RaycastAll(
                new PointerEventData(EventSystem.current) { position = position },
                Hits
            );

            return Hits.Any(hit => hit.gameObject.GetComponentInParent<Selectable>() != null);
        }
    }
}
