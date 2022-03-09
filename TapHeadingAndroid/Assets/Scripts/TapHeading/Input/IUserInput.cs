using UnityEngine;

namespace TapHeading.Input
{
    public interface IUserInput
    {
        void AddListener(IPlayerInputListener listener);
        void RemoveListener(IPlayerInputListener listener);
    }

    public interface IPlayerInputListener
    {
        void OnClick(Vector2 position);
    }
}