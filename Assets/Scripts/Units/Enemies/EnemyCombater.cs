using UnityEngine;

public class EnemyCombater : MonoBehaviour
{
    [Header("Attack Settings")]
    [SerializeField] private float attackRange = 2f;
    [SerializeField] private float attackRate = 1f;
    [SerializeField] private float attackDamage = 10f;
    
    [Header("References")]
    private ITargetProvider targetProvider;
    
    private float nextAttackTime;

    private void Start()
    {
        targetProvider = GetComponent<ITargetProvider>();
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
    
    public void Attack()
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
            
        DealDamageToTarget(target);
    }
    
    private void DealDamageToTarget(Transform target)
    {
        if (target.TryGetComponent(out PlayerHealth playerHealth))
            playerHealth.TakeDamage(attackDamage);
    }
    
    private Transform GetTarget()
    {
        return targetProvider?.GetCurrentTarget();
    }
    
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}