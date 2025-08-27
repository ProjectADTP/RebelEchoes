using UnityEngine;

public class EnemyHealth : Health
{
    [Header("Enemy Settings")]
    [SerializeField] private MonoBehaviour[] componentsToDisableOnDeath;
    [SerializeField] private EnemyAnimationController  _enemyAnimationController;
    [SerializeField] private Animator animator;

    public override void TakeDamage(float damage)
    {
        if (_enemyAnimationController != null)
        {
            _enemyAnimationController.TriggerHit();
        }
        
        base.TakeDamage(damage);
    }

    protected override void Die()
    {
        if (isDead) 
            return;
        
        isDead = true;
        
        if (_enemyAnimationController != null)
        {
            _enemyAnimationController.TriggerDeath();
        }
        
        OnEntityDied?.Invoke();
        
        DisableComponents();
        
        float deathAnimationLength = GetDeathAnimationLength();
        Destroy(_enemyAnimationController.gameObject, deathAnimationLength);
    }
    
    private void DisableComponents()
    {
        if (componentsToDisableOnDeath == null)
            return;
        
        foreach (var component in componentsToDisableOnDeath)
        {
            if (component != null)
                component.enabled = false;
        }
    }
    private float GetDeathAnimationLength()
    {
        AnimationClip[] clips = animator.runtimeAnimatorController.animationClips;
        
        foreach (AnimationClip clip in clips)
        {
            if (clip.name.Contains("Death"))
            {
                return clip.length + 1f;
            }
        }

        return 1.5f;
    }
}