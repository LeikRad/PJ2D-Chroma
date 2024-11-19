using UnityEngine;
using System.Collections;

[RequireComponent(typeof(PlayerController))]
public class Player : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public float jumpHeight = 4;
    public float timeToApex = .4f;

    public float moveSpeed = 6f;

    float gravity;
    float jumpVelocity;
    private bool canMove = true;
    private Vector3 velocity;
    private int movDirection = 1;
    PlayerController controller;

    public Animator animator;

    void Start()
    {
        controller = GetComponent<PlayerController>();

        gravity = -(2 * jumpHeight) / Mathf.Pow(timeToApex, 2);
        jumpVelocity = Mathf.Abs(gravity) * timeToApex;
    }

    // Update is called once per frame
    void Update()
    {
        animator.SetFloat("Speed", Mathf.Abs(velocity.x));

        if (canMove)
        {
            if (controller.collisions.above || controller.collisions.below)
            {
                velocity.y = 0;
                if (controller.collisions.below)
                {
                    animator.SetBool("IsJumping", false);
                }
            }
            
            Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
            if (input.x > 0.01f && movDirection == -1)
            {
                // make player face right
                movDirection = 1;
            }
            else if (input.x < -0.01f && movDirection == 1)
            {
                // make player face left
                movDirection = -1;
            }


            if (Input.GetKeyDown(KeyCode.Space) && controller.collisions.below)
            {
                velocity.y = jumpVelocity;
                animator.SetBool("IsJumping", true);
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
        Debug.Log("Direction " + direction);
        velocity.x = movDirection * direction * 10;
        velocity.y = jumpVelocity / 2;
        StartCoroutine(KnockbackTimer());
    }
    
    private IEnumerator KnockbackTimer()
    {
        yield return new WaitForSeconds(0.4f);
        canMove = true;
    }
}
