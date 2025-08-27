using UnityEngine;

public class EnemyAnimationController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Animator animator;
    [SerializeField] private EnemyMover enemyMovement;
    [SerializeField] private EnemyCombater enemyCombat;
    
    private static readonly int SpeedHash = Animator.StringToHash("Speed");
    private static readonly int IsAttackingHash = Animator.StringToHash("IsAttacking");
    private static readonly int IsHitHash = Animator.StringToHash("Hit");
    private static readonly int IsDeadHash = Animator.StringToHash("Dead");

    private bool isHit;
    private bool isDead;
    private bool isMovementDisabled;

    private void Start()
    {
        if (animator == null)
            animator = GetComponent<Animator>();
            
        enemyMovement = GetComponent<EnemyMover>();
        enemyCombat = GetComponent<EnemyCombater>();
        
        isHit = false;
        isDead = false;
        isMovementDisabled = false;
    }

    private void Update()
    {
        if (isDead) return;

        UpdateMovementAnimation();
        UpdateCombatAnimation();
    }

    private void UpdateMovementAnimation()
    {
        if (!animator || isMovementDisabled) 
            return;
        
        if (enemyMovement is EnemyMover mover && mover.IsAtTarget())
        {
            animator.SetFloat(SpeedHash, 0f);
        }
        else if (enemyMovement && enemyMovement.IsChasing())
        {
            animator.SetFloat(SpeedHash, 1f);
        }
        else
        {
            animator.SetFloat(SpeedHash, 0f);
        }
    }

    private void UpdateCombatAnimation()
    {
        if (!enemyCombat || !animator) 
            return;
        
        bool isAttacking = enemyCombat.IsPlayerInRange();
        animator.SetBool(IsAttackingHash, isAttacking);
    }

    public void TriggerHit()
    {
        if (isDead || isHit) 
            return;

        isHit = true;
        
        if (animator != null)
        {
            animator.SetTrigger(IsHitHash);
        }
        
        if (enemyMovement != null)
        {
            isMovementDisabled = true;
            
            enemyMovement.SetChasing(false);
        }
        
        Invoke(nameof(ResetHit), 0.1f);
    }

    private void ResetHit()
    {
        isHit = false;
        isMovementDisabled = false;
        
        if (enemyMovement != null)
        {
            enemyMovement.SetChasing(true);
        }
    }

    public void TriggerDeath()
    {
        if (isDead) 
            return;

        isDead = true;
        
        if (animator != null)
        {
            animator.SetBool(IsDeadHash, true);
        }

        if (enemyMovement is MonoBehaviour movementBehaviour)
            movementBehaviour.enabled = false;

        if (enemyCombat is MonoBehaviour combatBehaviour)
            combatBehaviour.enabled = false;
    }
}