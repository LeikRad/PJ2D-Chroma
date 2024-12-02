using System;
using UnityEngine;
using System.Collections;

[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public class Player : MonoBehaviour
{

    // Basic Movemnt
    public float moveSpeed = 6f;
    public float jumpVelocity = 8f;
    public float terminalVelocity = -10f;
    private float horizontal;
    private bool isFacingRight = true;
    
    // Jumps
    public int maxJumps = 2;
    private int jumpCount = 0;
    
    // Dash
    public float dashingForce = 10f;
    public float dashingTime = 0.2f;
    public float dashingCooldown = 1f;
    private bool canDash = true;
    private bool isDashing;
    
    // Wall Sliding and Jumping
    public float wallSlideSpeed = 1f;
    public float wallJumpingTime = 0.2f;
    public float wallJumpingDuration = 0.1f;
    public Vector2 wallJumpingForce = new Vector2(8f, 8f); 
    private bool isWallJumping;
    private float wallJumpingDirection;
    private float wallJumpingCounter;
    private bool isWallSliding;
    
    
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private Transform wallCheck;

    private bool canMove = true;
    private Vector3 velocity;
    private bool isGrounded;
    public Animator animator;

    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (isDashing || !canMove)
        {
            return;
        }
        IsGroundedCheck();

        // limit player falling speed
        if (rb.linearVelocityY < terminalVelocity)
        {
            rb.linearVelocityY = terminalVelocity;
        }
        
        if (isGrounded)
        {
            // TODO: fix bug here of animation
            jumpCount = 0;
            animator.SetBool("IsJumping", false);
        }
        
        horizontal = Input.GetAxisRaw("Horizontal");
        animator.SetFloat("Speed", Mathf.Abs(horizontal));
        
        if (Input.GetButtonDown("Jump"))
        {
            Jump();
        }
        
        if (Input.GetButtonDown("Jump") && rb.linearVelocityY > 0){
            rb.linearVelocity = new Vector2(rb.linearVelocityX, rb.linearVelocityY * 0.5f);
        }
        
        // TODO: Input map this 
        if (Input.GetKeyDown(KeyCode.LeftShift) && canDash)
        {
            StartCoroutine(Dash());
        }
        
        WallSlide();
        WallJump();
        if (!isWallJumping)
        {
            Flip();
        }
    }

    private void Jump()
    {
        if (jumpCount >= maxJumps)
        {
            return;
        }
        jumpCount++;
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpVelocity);
        animator.SetBool("IsJumping", true);
        return;
    }

    private void WallJump()
    {
        if (isWallSliding)
        {
            isWallJumping = false;
            wallJumpingDirection = -transform.localScale.x;
            wallJumpingCounter = wallJumpingTime;
            
            CancelInvoke(nameof(StopWallJumping));
        }
        else
        {
            wallJumpingCounter -= Time.deltaTime;
        }
        
        if (Input.GetButton("Jump") && wallJumpingCounter > 0)
        {  
            isWallJumping = true;
            rb.linearVelocity = new Vector2(wallJumpingForce.x * wallJumpingDirection, wallJumpingForce.y);
            wallJumpingCounter = 0f;
            
            if (transform.localScale.x != wallJumpingDirection)
            {
                isFacingRight = !isFacingRight;
                Vector3 localScale = transform.localScale;
                localScale.x *= -1;
                transform.localScale = localScale;
            }
            
            Invoke(nameof(StopWallJumping), wallJumpingDuration);
        }
    }

    private void StopWallJumping()
    {
        isWallJumping = false;
    }
    private void FixedUpdate()
    {
        if (isDashing)
        {
            return;
        }
        if (!isWallJumping )
        {
            rb.linearVelocity = new Vector2(horizontal * moveSpeed, rb.linearVelocityY);
        }
    }

    private void IsGroundedCheck()
    { 
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.1f, groundLayer);
    }
    
    private bool IsWalledCheck()
    {
        return Physics2D.OverlapCircle(wallCheck.position, 0.2f, groundLayer);
    }
    
    private void WallSlide()
    {
        if (IsWalledCheck() && !isGrounded && horizontal != 0)
        {
            isWallSliding = true;
            rb.linearVelocity = new Vector2(rb.linearVelocity.x,
                Mathf.Clamp(rb.linearVelocity.y, -wallSlideSpeed, float.MaxValue));
        }
        else
        {
            isWallSliding = false;
        }
    }

    private IEnumerator Dash()
    {
        canDash = false;
        isDashing = true;
        float originalGravity = rb.gravityScale;
        rb.gravityScale = 0;
        rb.linearVelocity = new Vector2(transform.localScale.x * dashingForce, 0f);
        yield return new WaitForSeconds(dashingTime);
        rb.gravityScale = originalGravity;
        isDashing = false;
        yield return new WaitForSeconds(dashingCooldown);
        canDash = true;
    }

    private void Flip()
    {
        if (isFacingRight && horizontal < 0 || !isFacingRight && horizontal > 0)
        {
            isFacingRight = !isFacingRight;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1;
            transform.localScale = localScale;
            // rotate weaponholder
            Debug.Log(transform.GetChild(0));
            transform.GetChild(0).Rotate(0f, 180f, 0f);
        }
    }

    // if (canMove)
        // {
        //     if (controller.collisions.above || controller.collisions.below)
        //     {
        //         velocity.y = 0;
        //         animator.SetBool("IsJumping", false);
        //     }
        //     
        //     Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        //     if (input.x > 0.01f && movDirection == -1)
        //     {
        //         // make player face right
        //         rotatePlayer();
        //         movDirection = 1;
        //     }
        //     else if (input.x < -0.01f && movDirection == 1)
        //     {
        //         // make player face left
        //         rotatePlayer();
        //         movDirection = -1;
        //     }
        //
        //     if (Input.GetKeyDown(KeyCode.Space) && controller.collisions.below)
        //     {
        //         velocity.y = jumpVelocity;
        //         animator.SetBool("IsJumping", true);
        //     }
        //
        //     velocity.x = input.x * moveSpeed;
        // }
        //
        // //sprites to make player walk
        // animator.SetFloat("Speed", Mathf.Abs(velocity.x));
        // velocity.y += gravity * Time.deltaTime;
        // controller.Move(velocity * Time.deltaTime);
    // }
    
    public void Knockback(Transform attacker)
    {
        // TODO: Fix this bug fast
        canMove = false;
        Vector2 difference = (transform.position - attacker.transform.position).normalized;
        Debug.Log("Diff: " + difference);
        Vector2 force = difference * 10;
        Debug.Log("Force: " + force);
        rb.linearVelocity = Vector2.zero; // Reset current velocity
        rb.AddForce(force, ForceMode2D.Impulse);
        StartCoroutine(KnockbackTimer());
        animator.SetBool("IsHurt", true);
    }
    
    private IEnumerator KnockbackTimer()
    {
        yield return new WaitForSeconds(0.4f);
        canMove = true;
        animator.SetBool("IsHurt", false);
    }
    
}