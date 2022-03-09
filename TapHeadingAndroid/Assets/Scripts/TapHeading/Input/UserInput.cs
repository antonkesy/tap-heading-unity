using System.Collections.Generic;
using UnityEngine;

namespace TapHeading.Input
{
    public abstract class UserInput : MonoBehaviour, IUserInput
    {
        private readonly List<IPlayerInputListener> _listeners = new List<IPlayerInputListener>();

        public void AddListener(IPlayerInputListener listener)
        {
            _listeners.Add(listener);
        }

        public void RemoveListener(IPlayerInputListener listener)
        {
            _listeners.Remove(listener);
        }

        protected void Notify(Vector2 position)
        {
            foreach (var observer in _listeners)
            {
                observer.OnClick(position);
            }
        }

        private void Update()
        {
            ProcessInput();
        }

        protected abstract void ProcessInput();
    }
}