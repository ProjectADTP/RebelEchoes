using UnityEngine;

public class PlayerCombat : MonoBehaviour, ITargetProvider
{
    [Header("Attack Settings")]
    [SerializeField] private float attackRadius = 10f;
    [SerializeField] private float attackRate = 1f;
    [SerializeField] private float combatRotationSpeed = 8f;
    
    [Header("Projectile")]
    [SerializeField] private Projectile projectilePrefab;
    [SerializeField] private Transform firePoint;
    [SerializeField] private float projectileSpeed = 20f;
    
    [Header("Targeting")]
    [SerializeField] private LayerMask enemyLayerMask = 8;
    
    private float nextAttackTime;
    private Transform currentTarget;
    private PlayerAnimationController animationController;
    private Collider[] colliders;
    
    private void Awake()
    {
        colliders = new Collider[25];
        
        animationController = GetComponent<PlayerAnimationController>();
    }

    private void Update()
    {
        HandleCombat();
    }

    private void HandleCombat()
    {
        Transform target = FindClosestEnemy();
        
        if (target)
        {
            currentTarget = target;

            if (!(Time.time >= nextAttackTime)) 
                return;
            
            AttackTarget(target);
            nextAttackTime = Time.time + 1f / attackRate;
        }
        else
        {
            animationController.SetShoot(false);
            currentTarget = null;
        }
    }
    
    private void AttackTarget(Transform target)
    {
        RotateToTarget(target);
        Shoot(target);
    }
    
    private void RotateToTarget(Transform target)
    {
        Vector3 directionToTarget = (target.position - transform.position).normalized;
        directionToTarget.y = 0f;

        if (directionToTarget == Vector3.zero) 
            return;
        
        Quaternion targetRotation = Quaternion.LookRotation(directionToTarget);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, combatRotationSpeed * Time.deltaTime);
    }
    
    private void Shoot(Transform target)
    {
        if (!projectilePrefab || !firePoint || !target)
        {
            return;    
        }

        animationController.SetShoot(true);
        
        Projectile projectile = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
        
        Vector3 direction = (target.position - firePoint.position).normalized;

        if (projectile.TryGetComponent(out Rigidbody projectileRigidbody))
        {
            projectileRigidbody.velocity = direction * projectileSpeed;
        }
    }
    
    private Transform FindClosestEnemy()
    {
        int colliderCount = Physics.OverlapSphereNonAlloc(transform.position, attackRadius, colliders, enemyLayerMask);
    
        Transform closestEnemy = null;
        float closestDistance = Mathf.Infinity;
    
        for (int i = 0; i < colliderCount; i++)
        {
            Collider enemy = colliders[i];
            
            if (!enemy.TryGetComponent(out EnemyHealth health) || !health.enabled || !health.IsAlive())
                continue;
        
            float distance = Vector3.Distance(transform.position, enemy.transform.position);

            if (!(distance < closestDistance))
                continue;
            
            closestDistance = distance;
            closestEnemy = enemy.transform;
        }
    
        return closestEnemy;
    }
    
    public bool HasTarget()
    {
        return currentTarget;
    }
    
    public Transform GetCurrentTarget()
    {
        return currentTarget;
    }
    
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRadius);
    }
}