using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerLocomotion : MonoBehaviour
{
    PlayerInput playerInput;
    public Rigidbody2D rigidBody;
    public float gravity = 3;

    [SerializeField]
    private float maxSpeed = 3;
    public float moveSpeed = 3;
    public float jumpForce = 3;

    public bool isGrounded;
    public float groundCheckRadius;
    [HideInInspector] public int maxJumps = 1;
    [SerializeField]private int remainingJumps;
    [SerializeField] float timeSinceFalling;
    public float coyoteTime = 0.3f;
    public LayerMask groundLayer;

    public float dodgeForce = 3;

    private bool facingRight = true;
    float aerialDrift;
    public float aerialDriftModifier;
    public float maxAerialSpeed;

    private void Awake()
    {
        rigidBody= GetComponent<Rigidbody2D>();
        playerInput = GetComponent<PlayerInput>();
    }

    private void Update()
    {
        HandleGroundedCheck();
    }

    public void HandleAllMovement()
    {
        HandleWalking();
        HandleJump();
    }

    private void FlipDirection()
    {
        facingRight = !facingRight;
        Vector3 scaler = transform.localScale;
        scaler.x *= -1;
        transform.localScale = scaler;
    }

    private void HandleJump()
    {
        remainingJumps = Mathf.Clamp(remainingJumps, 0, maxJumps);
        if (playerInput.performJump == 0 && rigidBody.velocity.y > 0)
        {
            rigidBody.velocity = new Vector2(rigidBody.velocity.x, -1);
        }
        if (!isGrounded)
        {
            timeSinceFalling += Time.deltaTime;
            aerialDrift = playerInput.movementInput.x * aerialDriftModifier;
            if (playerInput.movementInput.x != 0 && rigidBody.velocity.x <= maxAerialSpeed) { rigidBody.velocity += new Vector2(aerialDrift, 0); }
            return;
        }
        
        if (remainingJumps <= 0) return;

        if (playerInput.performJump != 0 && remainingJumps > 0)
        {
            isGrounded = false;
            remainingJumps--;
            rigidBody.AddRelativeForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }
    }

    private void HandleWalking()
    {
        float currentSpeed = playerInput.movementInput.x * moveSpeed * Time.fixedDeltaTime;
        float clampedSpeed = Mathf.Clamp(currentSpeed, -maxSpeed, maxSpeed);
        Vector2 velocity = new Vector2(clampedSpeed, rigidBody.velocity.y);
        if (!isGrounded) return;
        if (playerInput.movementInput.x < 0 && facingRight) FlipDirection();
        else if(playerInput.movementInput.x > 0 && !facingRight) FlipDirection();
        rigidBody.velocity = velocity;
    }

    private void HandleGroundedCheck()
    {
        if (rigidBody.velocity.y <= 0 && timeSinceFalling >= coyoteTime) isGrounded = IsGrounded();
        if (isGrounded)
        {
            timeSinceFalling = 0;
            remainingJumps = maxJumps;
        }
    }

    private bool IsGrounded()
    {
        Collider2D result = Physics2D.OverlapCircle(transform.position, groundCheckRadius, groundLayer);
        return result;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, groundCheckRadius);
    }
}
