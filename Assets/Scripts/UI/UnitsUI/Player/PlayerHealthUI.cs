using UnityEngine;

public class PlayerHealthUI : MonoBehaviour
{
    [SerializeField] private HealthBar healthBar;
    [SerializeField] private PlayerHealth playerHealth;

    private void Start()
    {
        if (playerHealth != null)
        {
            playerHealth.OnHealthChanged += UpdateHealthUI;
            UpdateHealthUI(playerHealth.GetCurrentHealth(), playerHealth.GetMaxHealth());
        }
    }

    private void UpdateHealthUI(float currentHealth, float maxHealth)
    {
        if (healthBar)
        {
            healthBar.SetMaxHealth(maxHealth);
            healthBar.SetHealth(currentHealth);
        }
    }

    private void OnDestroy()
    {
        if (playerHealth != null)
        {
            playerHealth.OnHealthChanged -= UpdateHealthUI;
        }
    }
}