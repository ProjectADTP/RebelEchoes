using System;
using UnityEngine;

public class PlayerDetector : MonoBehaviour, ITargetProvider
{
    [Header("Detection Settings")]
    [SerializeField] private float detectionRadius = 7f;

    [SerializeField] private LayerMask playerLayerMask = 7;
    [SerializeField] private float detectionCheckRate = 1f;
    
    private Transform player;
    private float nextDetectionTime;
    private Collider[] colliders;
    
    public event Action OnPlayerDetected;

    private void Awake()
    {
        colliders = new Collider[10];
    }

    private void Update()
    {
        if (!(Time.time >= nextDetectionTime))
            return;
        
        CheckForPlayer();
        nextDetectionTime = Time.time + 1f / detectionCheckRate;
    }
    
    private void CheckForPlayer()
    {
        int colliderCount = Physics.OverlapSphereNonAlloc(transform.position, detectionRadius, colliders, playerLayerMask);
    
        bool playerCurrentlyDetected = false;
        Transform detectedPlayer = null;
    
        for (int i = 0; i < colliderCount; i++)
        {
            Collider playerCollider = colliders[i];

            if (!playerCollider || !playerCollider.CompareTag("Player")) 
                continue;
            
            if (!playerCollider.TryGetComponent(out PlayerHealth playerHealth) || !playerHealth.IsAlive())
                continue;
                
            playerCurrentlyDetected = true;
            detectedPlayer = playerCollider.transform;
                    
            break;
        }
        
        if (playerCurrentlyDetected)
        {
            player = detectedPlayer;
            
            OnPlayerDetected?.Invoke();
        }
        else 
        {
            player = null;
        }
    }

    public Transform GetCurrentTarget()
    {
        return player;
    }
    
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}