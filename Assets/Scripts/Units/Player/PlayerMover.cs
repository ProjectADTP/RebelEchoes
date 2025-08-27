using UnityEngine;

public class PlayerMover : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float gravity = -9.81f;
    [SerializeField] private float rotationSpeed = 10f;
    
    [Header("References")]
    [SerializeField] private CharacterController characterController;
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private PlayerCombat playerCombat;
    
    private Vector3 velocity;
    private Vector2 movementInput = Vector2.zero;
    private bool hasMovementInput;

    public bool IsMoving => hasMovementInput;
    
    private void Start()
    {
        if (cameraTransform == null)
            cameraTransform = Camera.main?.transform;
    }

    private void Update()
    {
        HandleMovement();
    }

    private void HandleMovement()
    {
        Vector3 move = Vector3.zero;
        bool shouldRotate = !playerCombat.HasTarget();
        
        hasMovementInput = movementInput.sqrMagnitude > 0.1f;
        
        if (hasMovementInput)
        {
            float horizontal = movementInput.x;
            float vertical = movementInput.y;
            
            move = new Vector3(horizontal, 0, vertical);
            
            if (move.magnitude > 1f)
                move.Normalize();
            
            if (cameraTransform)
            {
                move = cameraTransform.TransformDirection(move);
                move.y = 0f;
            }
            
            if (shouldRotate)
            {
                Quaternion toRotation = Quaternion.LookRotation(move, Vector3.up);
                transform.rotation = Quaternion.Lerp(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);
            }
        }

        if (!shouldRotate)
        {
            Vector3 directionToTarget = (playerCombat.GetCurrentTarget().position - transform.position).normalized;
            directionToTarget.y = 0f;
        
            if (directionToTarget != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(directionToTarget);
                transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            }
        }
        
        ApplyMovement(move);
    }

    private void ApplyMovement(Vector3 moveDirection)
    {
        if (!characterController) 
            return;
        
        characterController.Move(moveDirection * moveSpeed * Time.deltaTime);
        
        velocity.y += gravity * Time.deltaTime;
        characterController.Move(velocity * Time.deltaTime);
            
        if (characterController.isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }
    }
    
    public void SetMovementInput(Vector2 input)
    {
        movementInput = input;
    }
}