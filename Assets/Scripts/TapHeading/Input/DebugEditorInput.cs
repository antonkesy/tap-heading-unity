using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace TapHeading.Input
{
    public class DebugEditorInput : UserInput
    {
        protected override void ProcessInput()
        {
            if (UnityEngine.Input.GetKeyDown(KeyCode.Space)) Notify(Vector2.zero);

            if (UnityEngine.Input.GetKeyDown(KeyCode.LeftArrow))
                Notify(Vector2.left);
            else if (UnityEngine.Input.GetKeyDown(KeyCode.RightArrow))
                Notify(Vector2.right);

            if (UnityEngine.Input.GetMouseButtonDown(0))
            {
                if (EventSystem.current.IsPointerOverGameObject() &&
                    EventSystem.current.currentSelectedGameObject != null &&
                    EventSystem.current.currentSelectedGameObject.GetComponent<Button>() == null)
                {
                    Debug.Log("UI click");
                }
            }
        }
    }
}