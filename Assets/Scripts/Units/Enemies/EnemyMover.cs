
using UnityEngine;

public class EnemyMover : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 3f;
    [SerializeField] private float rotationSpeed = 5f;
    [SerializeField] private float stoppingDistance = 1.5f;
    [SerializeField] private float gravity = -9.81f;
    
    [Header("References")]
    [SerializeField] private PlayerDetector targetProvider;
    
    private CharacterController characterController;
    private Vector3 velocity;
    private Transform target;
    
    private bool isChasing;
    private bool isAtTarget;
    
    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
        targetProvider = GetComponent<PlayerDetector>();
        
        isChasing  = false;
        isAtTarget = false;
    }

    private void OnEnable()
    {
        if (targetProvider != null)
        {
            targetProvider.OnPlayerDetected += OnPlayerDetected;
        }
    }

    private void Update()
    {
        ApplyGravity();
        
        if (isChasing && !isAtTarget)
        {
            ChaseTarget();
        }
    }

    private void OnDisable()
    {
        if (targetProvider != null)
        {
            targetProvider.OnPlayerDetected -= OnPlayerDetected;
        }
    }

    private void ChaseTarget()
    {
        if (!target)
        {
            SetChasing(false);
            isAtTarget = false;
            
            return;
        }
        
        Vector3 directionToTarget = (target.position - transform.position).normalized;
        float distanceToTarget = Vector3.Distance(transform.position, target.position);
        
        if (directionToTarget != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(directionToTarget);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
        
        if (distanceToTarget <= stoppingDistance)
        {
            isAtTarget = true;
            isChasing = false;
        }
        else
        {
            isAtTarget = false;
            
            Vector3 moveDirection = directionToTarget;
            moveDirection.y = 0f;
            
            if (characterController)
            {
                characterController.Move(moveDirection * moveSpeed * Time.deltaTime);
            }
        }
    }
    
    private void ApplyGravity()
    {
        if (!characterController) 
            return;
        
        velocity.y += gravity * Time.deltaTime;
        characterController.Move(velocity * Time.deltaTime);
            
        if (characterController.isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }
    }
    
    public bool IsChasing()
    {
        return isChasing;
    }

    public void SetChasing(bool status)
    {
        if (status)
        {
            if (!targetProvider || !targetProvider.GetCurrentTarget())
                return;
            
            isChasing = true;
        }
        else
        {
            isChasing = false;
        }

        isAtTarget = false;
    }
    
    public bool IsAtTarget()
    {
        return isAtTarget;
    }
    
    private void OnPlayerDetected()
    {
        SetChasing(true);
        
        target = targetProvider.GetCurrentTarget();
    }
}