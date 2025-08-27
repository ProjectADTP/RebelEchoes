using UnityEngine;
using UnityEngine.Events;

public abstract class Health : MonoBehaviour
{
    [Header("Health Settings")]
    [SerializeField] protected float maxHealth = 100f;
    [SerializeField] protected float currentHealth;
    
    [Header("Events")]
    public UnityEvent<float> OnHealthChanged;
    public UnityEvent OnEntityDied;
    
    protected bool isDead = false;

    protected virtual void Start()
    {
        currentHealth = maxHealth;
    }
    
    public virtual void TakeDamage(float damage)
    {
        if (isDead || damage <= 0) return;
        
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        
        OnHealthChanged?.Invoke(currentHealth);
        
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