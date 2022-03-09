using UnityEngine;
using UnityEngine.EventSystems;

namespace tap_heading.input
{
    public class DebugEditorInput : UserInput
    {
        protected override void ProcessInput()
        {
            if (Input.GetKeyDown(KeyCode.Space)) Notify(Vector2.zero);

            if (Input.GetKeyDown(KeyCode.LeftArrow))
                Notify(Vector2.left);
            else if (Input.GetKeyDown(KeyCode.RightArrow))
                Notify(Vector2.right);

            if (Input.GetMouseButtonDown(0))
            {
                if (EventSystem.current.IsPointerOverGameObject())
                {
                    Debug.Log("UI click");
                }
            }
        }
    }
}