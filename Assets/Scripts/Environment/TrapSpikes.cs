using UnityEngine;

public class TrapSpikes : MonoBehaviour
{
    [Header("Damage Settings")]
    [SerializeField] private int damage = 20;

    [Header("Components")]
    [SerializeField] private Collider spikesCollider;

    private CharacterController playerController;
    
    private bool isActive;

    private void Start()
    {
        if (spikesCollider != null)
            spikesCollider.enabled = false;

        playerController = null;
        isActive = false;
        
        gameObject.SetActive(false);
    }

    public void ActivateSpikes()
    {
        if (!isActive)
        {
            isActive = true;
            gameObject.SetActive(true);
            
            if (spikesCollider != null)
                spikesCollider.enabled = true;
        }
    }

    public void DeactivateSpikes()
    {
        if (isActive)
        {
            isActive = false;
            gameObject.SetActive(false);
            
            if (spikesCollider != null)
                spikesCollider.enabled = false;
                
            ReleasePlayer();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isActive && other.TryGetComponent<PlayerHealth>(out _))
        {
            HandlePlayerHit(other);
        }
        else if (isActive && other.TryGetComponent<EnemyMover>(out _))
        {
            HandleEnemyHit(other);
        }
    }

    private void HandlePlayerHit(Collider playerCollider)
    {
        var playerHealth = playerCollider.GetComponent<PlayerHealth>();
        var controller = playerCollider.GetComponent<CharacterController>();

        if (playerHealth != null && controller != null)
        {
            playerHealth.TakeDamage(damage);
            StunPlayer(controller);
        }
    }

    private void HandleEnemyHit(Collider enemyCollider)
    {
        var enemyHealth = enemyCollider.transform.GetChild(0).GetComponent<EnemyHealth>();
        var controller = enemyCollider.GetComponent<CharacterController>();

        if (enemyHealth != null && controller != null)
        {
            enemyHealth.TakeDamage(damage);
            StunPlayer(controller);
        }
    }

    private void StunPlayer(CharacterController controller)
    {
        playerController = controller;
        playerController.enabled = false;
    }

    private void ReleasePlayer()
    {
        if (playerController != null)
        {
            playerController.enabled = true;
            playerController = null;
        }
    }

    private void OnDisable()
    {
        ReleasePlayer();
    }
}