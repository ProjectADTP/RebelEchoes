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
    [SerializeField] private LayerMask obstacleLayerMask = 9;
    
    [SerializeField] private PlayerBuffs playerBuffs;
    
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
        
        int shotCount = playerBuffs ? playerBuffs.GetShotCount() : 1;
        float spreadAngle = playerBuffs ? playerBuffs.GetSpreadAngle() : 0f;

        if (shotCount == 1)
        {
            CreateProjectile(target, 0f);
        }
        else
        {
            float step = (spreadAngle * 2) / (shotCount - 1);

            for (int i = 0; i < shotCount; i++)
            {
                float angle = step * i;
                CreateProjectile(target, angle);
            }
        }
    }
    
    private void CreateProjectile(Transform target, float angleOffset)
    {
        Projectile projectile = Instantiate(projectilePrefab, firePoint.position, firePoint.localRotation);
        
        projectile.transform.localRotation = Quaternion.LookRotation((target.position - transform.position).normalized);
        
        if (angleOffset == 1)
        {
            projectile.transform.localPosition += projectile.transform.right * 2;
        }
        else if  (angleOffset == 2)
        {
            projectile.transform.localPosition -= projectile.transform.right * 2;
        }
        
        float damageMultiplier = playerBuffs ? playerBuffs.GetPowerMultiplier() : 1f;
        float lifetimeMultiplier = (playerBuffs && playerBuffs.IsBuffActive(BuffType.Power)) ? 2f : 1f;

        projectile.Initialize(
            target, 
            projectileSpeed, 
            damageMultiplier, 
            lifetimeMultiplier
        );
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
        
            Vector3 direction = enemy.transform.position - transform.position;
            
            float distance = Vector3.Distance(transform.position, enemy.transform.position);
            
            if (Physics.Raycast(transform.position, direction.normalized, distance, obstacleLayerMask))
                continue;
            
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