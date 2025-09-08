using System;
using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Animator animator;
    [SerializeField] private PlayerMover playerMover;
    [SerializeField] private PlayerHealth playerHealth;
    
    private static readonly int SpeedHash = Animator.StringToHash("Speed");
    private static readonly int IsShootingHash = Animator.StringToHash("Shoot");
    private static readonly int IsHitHash = Animator.StringToHash("Hit");
    private static readonly int IsDeadHash = Animator.StringToHash("Dead");

    private void OnEnable()
    {
        playerHealth.OnPlayerHit += TriggerHit;
        playerHealth.OnPlayerDied += TriggerDeath;
    }
    
    private void Update()
    {
        MovingUpdate();
    }
    
    private void OnDisable()
    {
        playerHealth.OnPlayerHit -= TriggerHit;
        playerHealth.OnPlayerDied -= TriggerDeath;
    }
    
    private void MovingUpdate()
    {
        if (playerHealth.IsAlive())
            animator.SetFloat(SpeedHash, playerMover.IsMoving ? 1f : 0f);
    }

    public void SetShoot(bool state)
    {
        if (playerHealth.IsAlive())
            animator.SetBool(IsShootingHash,state);
    }

    private void TriggerHit()
    {
        animator.SetTrigger(IsHitHash);
    }

    private void TriggerDeath()
    {
        animator.SetTrigger(IsDeadHash);
    }
}