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
    public bool canDash = true;
    public bool isDashing;
    
    // Wall Sliding and Jumping
    public float wallSlideSpeed = 1f;
    public float wallJumpingTime = 0.2f;
    public float wallJumpingDuration = 0.1f;
    public Vector2 wallJumpingForce = new Vector2(8f, 8f); 
    private bool isWallJumping;
    private float wallJumpingDirection;
    private float wallJumpingCounter;
    private bool isWallSliding;
    
    
    [SerializeField]  Rigidbody2D rb;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private Transform wallCheck;

    public bool canMove = true;
    private Vector3 velocity;
    private bool isGrounded;
    public Animator animator;

    private Health health;
    private PlayerWeapon playerWeapon;
    public static Player Instance { get; private set; }

    void Awake() {

        if(Instance == null) {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if(Instance != this) {
            Destroy(this);
        }
    }
    void Start()
    {
        health = GetComponent<Health>();
        playerWeapon = GetComponent<PlayerWeapon>();
    }

    // Update is called once per frame
    /*void Update()
    {
        if (health.currentHealth > 0)
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
                jumpCount = 0;
            }

            horizontal = Input.GetAxisRaw("Horizontal");
            animator.SetFloat("Speed", Mathf.Abs(horizontal));

            if (Input.GetButtonDown("Jump"))
            {
                Jump();
                animator.SetBool("IsJumping", true);

            }

            if (Input.GetButtonDown("Jump") && rb.linearVelocityY > 0)
            {
                rb.linearVelocity = new Vector2(rb.linearVelocityX, rb.linearVelocityY * 0.5f);
            }

            // TODO: Input map this 
            if (Input.GetKeyDown(KeyCode.LeftShift) && canDash)
            {
                StartCoroutine(Dash());
                animator.SetBool("isDash", true);
            }
            else
            {
                animator.SetBool("isDash", false);
            }

            WallSlide();
            WallJump();
            if (!isWallJumping)
            {
                Flip();
            }

            //jumping bug
            if (rb.linearVelocityY <= 0)
            {
                animator.SetBool("IsJumping", false);
            }
        }
    }
    */

    private void Jump()
    {
        if (jumpCount >= maxJumps)
        {
            return;
        }
        jumpCount++;
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpVelocity);
        //animator.SetBool("IsJumping", true);
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
    /*private void FixedUpdate()
    {
        if (isDashing)
        {
            return;
        }
        if (!isWallJumping )
        {
            rb.linearVelocity = new Vector2(horizontal * moveSpeed, rb.linearVelocityY);
        }
    }*/

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
            animator.SetBool("IsWallSlide", true);
        }
        else
        {
            isWallSliding = false;
            animator.SetBool("IsWallSlide", false);
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
            playerWeapon.weaponHolder.localScale = new Vector3(isFacingRight ? 1 : -1,1,1);
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
    
    public void Save(ref SaveSystem.SaveData data)
    {
        data.PlayerPosition = transform.position;
        data.PlayerHealth = GetComponent<PlayerHealth>().currentHealth;
        data.HasWeapon = GetComponent<PlayerWeapon>().equippedWeapon != null;
    }

    public void Load(SaveSystem.SaveData data)
    {
        transform.position = data.PlayerPosition;
        GetComponent<PlayerHealth>().currentHealth = data.PlayerHealth;

        if (data.HasWeapon)
        {
            GetComponent<PlayerWeapon>().EquipDefaultWeapon();
        }
    }
}

[System.Serializable]
public struct PlayerSaveData
{
    public Vector3 Position;
    public float Health;
    public bool HasWeapon;
    
}