using UnityEngine;
using UnityEngine.Events;

public class PlayerHealth : MonoBehaviour
{
    [Header("Health Settings")]
    [SerializeField] private float maxHealth = 100f;
    [SerializeField] private float currentHealth;
    
    [SerializeField] private MonoBehaviour[] componentsToDisableOnDeath;
    
    public UnityEvent<float> OnHealthChanged;
    public UnityEvent OnPlayerDied;
    public UnityEvent OnPlayerHit;
    
    private PlayerAnimationController animationController;
    
    private bool isDead;

    private void Start()
    {
        isDead = false;
        currentHealth = maxHealth;
        animationController = GetComponent<PlayerAnimationController>();
    }
    
    public void TakeDamage(float damage)
    {
        if (isDead || damage <= 0) return;
        
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        
        OnHealthChanged?.Invoke(currentHealth);
        OnPlayerHit?.Invoke();
        
        if (animationController)
        {
            animationController.TriggerHit();
        }
        
        if (currentHealth <= 0)
        {
            Die();
        }
    }
    
    private void Die()
    {
        if (isDead) return;
        
        isDead = true;
        
        if (animationController)
        {
            animationController.TriggerDeath();
        }
        
        OnPlayerDied?.Invoke();
        
        DisableComponents();
    }
    
    private void DisableComponents()
    {
        if (componentsToDisableOnDeath == null) 
            return;
        
        foreach (var component in componentsToDisableOnDeath)
        {
            if (component)
                component.enabled = false;
        }
    }
    
    public bool IsAlive()
    {
        return !isDead && currentHealth > 0;
    }
}