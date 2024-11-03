using System;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public Transform player;
    public float speed = 3f;
    public float detectionRange = 10f;
    public float attackRange = 1.5f;
    public float attackDamage = 10f;
    public float knockbackForce = 2f;
    public float attackCooldown = 1f;
    private float lastAttackTime = 0f;
    public Transform[] patrolPoints;
    private int currentPointIndex = 0;
    public float patrolPointTolerance = 0.5f;
    public float chaseCooldown = 2f;
    private float lastChaseTime = 0f;
    private Rigidbody2D rb;
    private bool isPlayerAlive = true;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (player == null || !isPlayerAlive)
        {
            Patrol();
            return;
        }

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        // if (distanceToPlayer < attackRange && Time.time >= lastAttackTime + attackCooldown)
        // {
        //     Attack();
        // }
        if (distanceToPlayer < detectionRange && Time.time >= lastChaseTime + chaseCooldown)
        {
            ChasePlayer();
        }
        else
        {
            Patrol();
        }
    }

    void ChasePlayer()
    {
        Vector3 direction = (player.position - transform.position).normalized;
        transform.position += direction * speed * Time.deltaTime;
        transform.position = new Vector3(transform.position.x, transform.position.y, 0);
    }

    void Patrol()
    {
        if (patrolPoints.Length == 0) return;

        Transform targetPoint = patrolPoints[currentPointIndex];
        Vector3 direction = (targetPoint.position - transform.position).normalized;
        transform.position += direction * speed * Time.deltaTime;

        if (Vector3.Distance(transform.position, targetPoint.position) < patrolPointTolerance)
        {
            currentPointIndex = (currentPointIndex + 1) % patrolPoints.Length;
        }
    }

    // void Attack()
    // {
    //     Health playerHealth = player.GetComponent<Health>();
    //     if (playerHealth != null)
    //     {
    //         playerHealth.TakeDamage(attackDamage);
    //
    //         if (playerHealth.currentHealth <= 0)
    //         {
    //             isPlayerAlive = false;
    //         }
    //     }
    //
    //     Vector2 knockbackDirection = (transform.position - player.position).normalized;
    //     rb.AddForce(knockbackDirection * knockbackForce, ForceMode2D.Impulse);
    //     Debug.Log("Knockback: " + knockbackDirection);
    //     Debug.Log("Knock Force" + knockbackForce);
    //     Debug.Log("Impulse: " + ForceMode2D.Impulse);
    //     lastAttackTime = Time.time;
    //     lastChaseTime = Time.time;
    // }

    private void OnCollisionEnter2D(Collision2D other)
    { 
        // compare tag of the object collided with
        if (other.gameObject.CompareTag("Player"))
        {
            Health playerHealth = other.gameObject.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(attackDamage, this.transform);

                if (playerHealth.GetHealth() <= 0)
                {
                    isPlayerAlive = false;
                }
            }
        }
    }
}
