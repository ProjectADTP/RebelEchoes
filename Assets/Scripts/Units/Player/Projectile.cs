using UnityEngine;

public class Projectile : MonoBehaviour
{
    [Header("Projectile Settings")]
    [SerializeField] private float baseDamage = 10f;
    [SerializeField] private float baseLifetime = 2f;
    
    private Transform target;
    private float speed;
    private float damageMultiplier = 1f;
    private float currentLifetime;
    private Vector3 direction;
    
    public void Initialize(Transform target, float speed, float damageMultiplier, float lifetimeMultiplier)
    {
        this.target = target;
        this.speed = speed;
        this.damageMultiplier = damageMultiplier;
        
        currentLifetime = baseLifetime * lifetimeMultiplier;
        
        if (target != null)
        {
            direction = (target.position - transform.position).normalized;
        }
    }

    private void Update()
    {
        transform.Translate(direction * speed * Time.deltaTime, Space.World);
        
        currentLifetime -= Time.deltaTime;
        
        if (currentLifetime <= 0)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out EnemyHealth health))
        {
            float finalDamage = baseDamage * damageMultiplier;
            health.TakeDamage(Mathf.RoundToInt(finalDamage));
        }
    }
}