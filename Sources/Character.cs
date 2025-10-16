using UnityEngine;

public class Character : MonoBehaviour
{
    public float walkSpeed = 5f;
    public float acceleration = 15f;
    public float airControl = 0.5f;

    public float jumpHeight = 2f;
    public float coyoteTime = 0.2f;

    public float gravity = -30f;
    public float groundCheckRadius = 0.3f;
    public LayerMask groundMask;
    
    private CharacterController controller;
    private Vector3 velocity;
    private Vector3 moveDirection;
    private bool isGrounded;
    private float coyoteTimeCounter;

    public void MoveByDirection(Vector3 direction)
    {
        direction.y = 0;
        moveDirection = new Vector3(direction.x, 0, direction.z).normalized;
    }
    
    private void Start()
    {
        controller = GetComponent<CharacterController>();
    }
    
    void Update()
    {
        HandleGroundCheck();
        HandleInput();
        HandleMovement();
        HandleJump();
        ApplyGravity();
        MoveCharacter();
    }
    
    void HandleGroundCheck()
    {
        isGrounded = Physics.CheckSphere(transform.position, 
            groundCheckRadius, groundMask);
        
        // Койот-тайм
        if (isGrounded)
        {
            coyoteTimeCounter = coyoteTime;
        }
        else
        {
            coyoteTimeCounter -= Time.deltaTime;
        }
    }
    
    void HandleInput()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        
        
        MoveByDirection(transform.TransformDirection(new Vector3(horizontal, 0, vertical)));
    }
    
    void HandleMovement()
    {
        float targetSpeed = GetTargetSpeed();
        Vector3 targetVelocity = moveDirection * targetSpeed;
        
        float controlFactor = isGrounded ? 1f : airControl;
        velocity.x = Mathf.Lerp(velocity.x, targetVelocity.x, acceleration * controlFactor * Time.deltaTime);
        velocity.z = Mathf.Lerp(velocity.z, targetVelocity.z, acceleration * controlFactor * Time.deltaTime);
    }
    
    float GetTargetSpeed()
    {
        return walkSpeed;
    }
    
    void HandleJump()
    {
        if (Input.GetButtonDown("Jump") && CanJump())
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }
    }
    
    bool CanJump()
    {
        return coyoteTimeCounter > 0 && isGrounded;
    }
    
    void ApplyGravity()
    {
        if (!isGrounded)
        {
            velocity.y += gravity * Time.deltaTime;
        }
        else if (velocity.y < 0)
        {
            velocity.y = -2f;
        }
        
        velocity.y = Mathf.Max(velocity.y, gravity * 2f);
    }
    
    void MoveCharacter()
    {
        controller.Move(velocity * Time.deltaTime);
    }
    
    // Визуализация в редакторе
    void OnDrawGizmosSelected()
    {
        Gizmos.color = isGrounded ? Color.green : Color.red;
        Gizmos.DrawWireSphere(transform.position, groundCheckRadius);
    }
}
