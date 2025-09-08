using System;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [Header("Health Settings")]
    [SerializeField] private float maxHealth = 100f;
    [SerializeField] private float currentHealth;
    
    [SerializeField] private MonoBehaviour[] componentsToDisableOnDeath;

    [SerializeField] private PlayerBuffs playerBuffs;
    
    public event Action<float, float> OnHealthChanged; 
    public event Action OnPlayerDied;
    public event Action OnPlayerHit;
    
    public float GetCurrentHealth() => currentHealth;
    public float GetMaxHealth() => maxHealth;
    
    private bool isDead;

    private void Awake()
    {
        isDead = false;
        currentHealth = maxHealth;
    }

    public void TakeDamage(float damage)
    {
        if (isDead || damage <= 0 || playerBuffs.IsGhostMode())
            return;
        
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        
        OnHealthChanged?.Invoke(currentHealth, maxHealth);
        
        OnPlayerHit?.Invoke();
        
        if (currentHealth <= 0)
        {
            Die();
        }
    }
    
    public void Heal(int amount)
    {
        if (!IsAlive()) 
            return;

        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        OnHealthChanged?.Invoke(currentHealth, maxHealth);
    }
    
    private void Die()
    {
        if (isDead)
            return;
        
        isDead = true;
        
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