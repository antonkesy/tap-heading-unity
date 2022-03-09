using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

namespace tap_heading.input
{
    public class TouchInput : UserInput
    {
        protected override void ProcessInput()
        {
            if (Input.touchCount <= 0 || Input.GetTouch(0).phase != TouchPhase.Began) return;
            if (Input.touches.Select(touch => touch.fingerId)
                .Any(id => EventSystem.current.IsPointerOverGameObject(id)))
                return;

            Notify(Input.GetTouch(Input.touchCount - 1).position);
        }
    }
}