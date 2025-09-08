using UnityEngine;

public class EnemyHealthBar : MonoBehaviour
{
    [SerializeField] private HealthBar healthBar;
    [SerializeField] private EnemyHealth enemyHealth;
    [SerializeField] private Canvas healthCanvas;

    private void Start()
    {
        if (enemyHealth == null)
            return;
        
        enemyHealth.OnHealthChanged += UpdateHealthBar;
        enemyHealth.OnEntityDied += HideHealthBar;

        if (healthBar == null)
            return;
        
        healthBar.SetMaxHealth(enemyHealth.GetMaxHealth());
        healthBar.SetHealth(enemyHealth.GetMaxHealth());
    }

    private void UpdateHealthBar(float current, float max)
    {
        if (healthBar != null)
        {
            healthBar.SetMaxHealth(max);
            healthBar.SetHealth(current);
        }

        if (healthCanvas != null && !healthCanvas.gameObject.activeSelf)
        {
            healthCanvas.gameObject.SetActive(true);
        }
    }

    private void HideHealthBar()
    {
        if (healthCanvas != null)
        {
            healthCanvas.gameObject.SetActive(false);
        }
    }

    private void OnDestroy()
    {
        if (enemyHealth != null)
        {
            enemyHealth.OnHealthChanged -= UpdateHealthBar;
            enemyHealth.OnEntityDied -= HideHealthBar;
        }
    }
}