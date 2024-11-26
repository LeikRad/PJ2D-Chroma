using UnityEngine;
using System.Collections;

[RequireComponent(typeof(PlayerController))]
public class Player : MonoBehaviour
{
    public float jumpHeight = 4;
    public float timeToApex = .4f;
    public float moveSpeed = 6f;
    public int maxJumps = 2;
    public float wallSlideSpeed = 2f;
    public Vector2 wallJumpClimb = new Vector2(7.5f, 16f);
    public Vector2 wallJumpOff = new Vector2(8.5f, 7f);
    public Vector2 wallLeap = new Vector2(18f, 17f);

    float gravity;
    float jumpVelocity;
    private bool canMove = true;
    private Vector3 velocity;
    private int movDirection = 1;
    private int jumpCount;
    private bool wallSliding;
    private int wallDirX;
    PlayerController controller;
    public float dashingPower = 24f;
    public float dashingTime = 0.2f;
    public float dashingCooldown = 1f;
    private bool canDash = true;
    private bool isDashing;
    [SerializeField] private Rigidbody2D rb;

    void Start()
    {
        controller = GetComponent<PlayerController>();

        gravity = -(2 * jumpHeight) / Mathf.Pow(timeToApex, 2);
        jumpVelocity = Mathf.Abs(gravity) * timeToApex;
        jumpCount = maxJumps;
    }

    void Update()
    {
        if (isDashing)
        {
            return;
        }
        if (canMove)
        {
            if (controller.collisions.above || controller.collisions.below)
            {
                velocity.y = 0;
            }

            if (controller.collisions.below)
            {
                jumpCount = maxJumps;
            }

            Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
            if (input.x > 0.01f && movDirection == -1)
            {
                rotatePlayer();
                movDirection = 1;
            }
            else if (input.x < -0.01f && movDirection == 1)
            {
                rotatePlayer();
                movDirection = -1;
            }

            wallDirX = (controller.collisions.left) ? -1 : 1;
            wallSliding = false;
            if ((controller.collisions.left || controller.collisions.right) && !controller.collisions.below && velocity.y < 0)
            {
                wallSliding = true;
                if (velocity.y < -wallSlideSpeed)
                {
                    velocity.y = -wallSlideSpeed;
                }
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (jumpCount > 0)
                {
                    velocity.y = jumpVelocity;
                    jumpCount--;
                }
                else if (wallSliding)
                {
                    if (wallDirX == input.x)
                    {
                        velocity.x = -wallDirX * wallJumpClimb.x;
                        velocity.y = wallJumpClimb.y;
                    }
                    else if (input.x == 0)
                    {
                        velocity.x = -wallDirX * wallJumpOff.x;
                        velocity.y = wallJumpOff.y;
                    }
                    else
                    {
                        velocity.x = -wallDirX * wallLeap.x;
                        velocity.y = wallLeap.y;
                    }
                }
            }
            if (Input.GetKeyDown(KeyCode.LeftShift) && canDash)
            {
                StartCoroutine(Dash());
            }
            velocity.x = input.x * moveSpeed;
        }

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    public void Knockback(Transform attacker)
    {
        canMove = false;
        int direction = transform.position.x < attacker.position.x ? -1 : 1;
        velocity.x = movDirection * direction * 10;
        velocity.y = jumpVelocity / 2;
        StartCoroutine(KnockbackTimer());
    }

    private IEnumerator Dash()
    {
        canDash = false;
        isDashing = true;
        float originalGravity = rb.gravityScale;
        rb.gravityScale = 0f;
        rb.linearVelocity = new Vector2(transform.localScale.x * dashingPower, 0f);
        yield return new WaitForSeconds(dashingTime);
        rb.linearVelocity = Vector2.zero;
        rb.gravityScale = originalGravity;
        isDashing = false;
        yield return new WaitForSeconds(dashingCooldown);
        canDash = true;
    }
    
    private IEnumerator KnockbackTimer()
    {
        yield return new WaitForSeconds(0.4f);
        canMove = true;
    }

    private void rotatePlayer()
    {
        transform.localScale = Vector3.Scale(transform.localScale, new Vector3(-1, 1, 1));
        foreach (Transform child in transform)
        {
            child.localScale = Vector3.Scale(child.localScale, new Vector3(-1, 1, 1));
        }
    }
}