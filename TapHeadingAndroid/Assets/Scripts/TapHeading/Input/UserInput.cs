using System.Collections.Generic;
using UnityEngine;

namespace TapHeading.Input
{
    public abstract class UserInput : MonoBehaviour, IUserInput
    {
        private readonly List<IPlayerInputListener> listeners = new List<IPlayerInputListener>();

        public void AddListener(IPlayerInputListener listener)
        {
            listeners.Add(listener);
        }

        public void RemoveListener(IPlayerInputListener listener)
        {
            listeners.Remove(listener);
        }

        protected void Notify(Vector2 position)
        {
            foreach (var observer in listeners)
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