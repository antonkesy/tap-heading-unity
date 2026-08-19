using UnityEngine;

namespace TapHeading.Game.Level.Obstacle
{
    public class Coin : MonoBehaviour
    {
        [SerializeField]
        private ParticleSystem particlePrefab;
        private ParticleSystem _pickupParticleSystem;

        [SerializeField]
        private GameObject[] sprites;

        private void Awake()
        {
            _pickupParticleSystem = Instantiate(particlePrefab, transform);
        }

        public void PickUp()
        {
            _pickupParticleSystem.Play();
            foreach (var sprite in sprites)
            {
                sprite.SetActive(false);
            }
        }

        //not named Reset: that is a Unity editor message and would fire on inspector reset
        public void Show()
        {
            foreach (var sprite in sprites)
            {
                sprite.SetActive(true);
            }
        }
    }
}
