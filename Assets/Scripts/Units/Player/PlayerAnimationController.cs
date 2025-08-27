using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Animator animator;
    [SerializeField] private PlayerMover playerMover;
    
    private static readonly int SpeedHash = Animator.StringToHash("Speed");
    private static readonly int IsShootingHash = Animator.StringToHash("Shoot");
    private static readonly int IsHitHash = Animator.StringToHash("Hit");
    private static readonly int IsDeadHash = Animator.StringToHash("Dead");

    private bool isDead;

    private void Awake()
    {
        if (animator == null)
            animator = GetComponent<Animator>();
        
        isDead = false;
    }

    private void Update()
    {
        MovingUpdate();
    }

    private void MovingUpdate()
    {
        if (isDead) 
            return;

        animator.SetFloat(SpeedHash, playerMover.IsMoving ? 1f : 0f);
    }

    public void SetShoot(bool state)
    {
        if (!isDead)
        {
            animator.SetBool(IsShootingHash,state);
        }
    }

    public void TriggerHit()
    {
        if (!isDead)
        {
            animator.SetTrigger(IsHitHash);
        }
    }

    public void TriggerDeath()
    {
        if (isDead) 
            return;
        
        isDead = true;
        
        animator.SetBool(IsDeadHash, true);
    }
}