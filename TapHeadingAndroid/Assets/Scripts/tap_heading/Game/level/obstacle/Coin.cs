using UnityEngine;

namespace tap_heading.Game.level.obstacle
{
    public class Coin : MonoBehaviour
    {
        [SerializeField] private ParticleSystem particlePrefab;
        private ParticleSystem pickupParticleSystem;
        [SerializeField] private GameObject[] sprites;

        private void Awake()
        {
            pickupParticleSystem = Instantiate(particlePrefab, transform).GetComponent<ParticleSystem>();
        }

        public void PickUp()
        {
            pickupParticleSystem.Play();
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