using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.EventSystems.EventSystem;

namespace TapHeading.Input
{
    public class TouchInput : UserInput
    {
        protected override void ProcessInput()
        {
            if (UnityEngine.Input.touchCount <= 0 || UnityEngine.Input.GetTouch(0).phase != TouchPhase.Began) return;
            //check if click on UI Button
            if (UnityEngine.Input.touches.Select(touch => touch.fingerId)
                .Any(id => current.IsPointerOverGameObject(id) &&
                           current.currentSelectedGameObject != null &&
                           (current.currentSelectedGameObject.GetComponent<Button>() != null ||
                            current.currentSelectedGameObject.GetComponentInChildren<Button>() != null)))
                return;

            Notify(UnityEngine.Input.GetTouch(UnityEngine.Input.touchCount - 1).position);
        }
    }
}