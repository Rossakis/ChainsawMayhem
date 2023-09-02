using UnityEngine;

namespace ChainsawMan
{
    public class HealthDrops : MonoBehaviour
    {
        [SerializeField] private float addedHealth;
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                other.GetComponentInParent<PlayerHealth>().AddHealth(addedHealth);
                Destroy(gameObject);
            }
        }
    }
}
