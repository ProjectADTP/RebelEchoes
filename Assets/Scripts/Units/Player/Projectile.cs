using UnityEngine;

public class Projectile : MonoBehaviour
{
    [Header("Projectile Settings")]
    [SerializeField] private float damage = 10f;
    [SerializeField] private float lifetime = 2f;
    
    private void Start()
    {
        Destroy(gameObject, lifetime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out EnemyHealth health))
        {
            health.TakeDamage(damage);
        }
    }
}