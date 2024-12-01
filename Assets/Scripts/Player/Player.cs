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

    void Start()
    {
        controller = GetComponent<PlayerController>();

        gravity = -(2 * jumpHeight) / Mathf.Pow(timeToApex, 2);
        jumpVelocity = Mathf.Abs(gravity) * timeToApex;
    }

    // Update is called once per frame
    void Update()
    {
        if (canMove)
        {
            if (controller.collisions.above || controller.collisions.below)
            {
                velocity.y = 0;
            }
            
            Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
            if (input.x > 0.01f && movDirection == -1)
            {
                // make player face right
                rotatePlayer();
                movDirection = 1;
            }
            else if (input.x < -0.01f && movDirection == 1)
            {
                // make player face left
                rotatePlayer();
                movDirection = -1;
            }

            if (Input.GetKeyDown(KeyCode.Space) && controller.collisions.below)
            {
                velocity.y = jumpVelocity;
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
        //animator.SetBool("IsHurt", true);
    }
    
    private IEnumerator KnockbackTimer()
    {
        yield return new WaitForSeconds(0.4f);
        canMove = true;
        animator.SetBool("IsHurt", false);
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