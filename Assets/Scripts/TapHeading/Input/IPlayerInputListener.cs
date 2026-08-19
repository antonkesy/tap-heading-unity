using UnityEngine;

namespace TapHeading.Input
{
    public interface IPlayerInputListener
    {
        void OnClick(Vector2 position);
    }
}
