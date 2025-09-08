using UnityEngine;

public class CombatMage : MonoBehaviour
{
    [Header("Attack Settings")]
    [SerializeField] private float attackRange = 10f;
    [SerializeField] private float attackRate = 0.5f;
    [SerializeField] private float projectileSpeed = 5f;
    [SerializeField] private float arcHeight = 3f;
    [SerializeField] private float attackDamage = 15f;
    [SerializeField] private Enemy enemy;
    
    [Header("Projectile Settings")]
    [SerializeField] private EnemyProjectile projectilePrefab;
    [SerializeField] private Transform projectileSpawnPoint;
    
    [Header("References")]
    private ITargetProvider targetProvider;
    
    private float nextAttackTime;

    private void Start()
    {
        targetProvider = GetComponent<ITargetProvider>();
        
        if (projectileSpawnPoint == null)
            projectileSpawnPoint = transform;
    }
    
    private void Update()
    {
        if (!(Time.time >= nextAttackTime) || !IsPlayerInRange()) 
            return;
        
        Attack();
        nextAttackTime = Time.time + 1f / attackRate;
    }
    
    public bool IsPlayerInRange()
    {
        Transform target = GetTarget();
        
        if (!target) 
            return false;
        
        float distanceToTarget = Vector3.Distance(transform.position, target.position);
        return distanceToTarget <= attackRange;
    }
    
    private void Attack()
    {
        Transform target = GetTarget();
        
        if (!target)
            return;
        
        Vector3 directionToTarget = (target.position - transform.position).normalized;
        
        if (directionToTarget != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(directionToTarget);
            transform.rotation = targetRotation;
        }
        
        LaunchProjectile(target);
    }
    
    private void LaunchProjectile(Transform target)
    {
        Debug.Log("Launching projectile");
        
        if (projectilePrefab == null)
            return;
        
        EnemyProjectile projectile = Instantiate(projectilePrefab, projectileSpawnPoint.position, Quaternion.identity);
        
        projectile.Initialize(target, projectileSpeed, arcHeight, attackDamage);
    }
    
    private Transform GetTarget()
    {
        return targetProvider?.GetCurrentTarget();
    }
    
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        
        if (projectileSpawnPoint != null)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawSphere(projectileSpawnPoint.position, 0.1f);
        }
    }
}