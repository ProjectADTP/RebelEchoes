using UnityEngine;
using System;

public abstract class Health : MonoBehaviour
{
    [Header("Health Settings")]
    [SerializeField] protected float maxHealth = 100f;
    [SerializeField] protected float currentHealth;
    
    public event Action<float,float> OnHealthChanged;

    protected bool isDead = false;
    
    public float GetMaxHealth() => 
        maxHealth;
    
    protected virtual void Start()
    {
        currentHealth = maxHealth;
    }
    
    public virtual void TakeDamage(float damage)
    {
        if (isDead || damage <= 0) return;
        
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        
        OnHealthChanged?.Invoke(currentHealth, maxHealth);
        
        if (currentHealth <= 0)
        {
            Die();
        }
    }
    
    protected abstract void Die();
    
    public bool IsAlive()
    {
        return !isDead && currentHealth > 0;
    }
}