using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

namespace TapHeading.Input
{
    public class TouchInput : UserInput
    {
        protected override void ProcessInput()
        {
            if (UnityEngine.Input.touchCount <= 0 || UnityEngine.Input.GetTouch(0).phase != TouchPhase.Began) return;
            if (UnityEngine.Input.touches.Select(touch => touch.fingerId)
                .Any(id => EventSystem.current.IsPointerOverGameObject(id)))
                return;

            Notify(UnityEngine.Input.GetTouch(UnityEngine.Input.touchCount - 1).position);
        }
    }
}