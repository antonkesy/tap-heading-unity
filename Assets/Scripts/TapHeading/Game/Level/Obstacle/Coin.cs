using UnityEngine;

namespace TapHeading.Game.Level.Obstacle
{
    public class Coin : MonoBehaviour
    {
        [SerializeField] private ParticleSystem particlePrefab;
        private ParticleSystem _pickupParticleSystem;
        [SerializeField] private GameObject[] sprites;

        private void Awake()
        {
            _pickupParticleSystem = Instantiate(particlePrefab, transform).GetComponent<ParticleSystem>();
        }

        public void PickUp()
        {
            _pickupParticleSystem.Play();
            foreach (var sprite in sprites)
            {
                sprite.SetActive(false);
            }
        }

        public void Reset()
        {
            foreach (var sprite in sprites)
            {
                sprite.SetActive(true);
            }
        }
    }
}