using UnityEngine;

namespace TapHeading.Input
{
    public class DebugEditorInput : UserInput
    {
        protected override void ProcessInput()
        {
            if (UnityEngine.Input.GetKeyDown(KeyCode.Space))
                Notify(Vector2.zero);

            //listeners compare against Screen.width, so send screen space, not unit vectors
            if (UnityEngine.Input.GetKeyDown(KeyCode.LeftArrow))
                Notify(Vector2.zero);
            else if (UnityEngine.Input.GetKeyDown(KeyCode.RightArrow))
                Notify(Vector2.right * Screen.width);
        }
    }
}
