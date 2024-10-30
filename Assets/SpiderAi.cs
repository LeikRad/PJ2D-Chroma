using UnityEngine;

public class SpiderAI : MonoBehaviour
{
    public Transform player; 
    public float speed = 3f; 
    public float detectionRange = 10f; 
    public float attackRange = 1.5f; 
    public float attackDamage = 25f; 
    public float attackCooldown = 1f; 
    private float _lastAttackTime = 0f;
    private Rigidbody2D _rb; 
    private bool _isPlayerAlive = true; 
    private Vector3 _initialPosition;

    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _initialPosition = transform.position; 
    }

    void Update()
    {
        if (player == null || !_isPlayerAlive)
        {
            MoveToInitialPosition(); 
            return; 
        }

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer < attackRange && Time.time >= _lastAttackTime + attackCooldown)
        {
            Attack();
        }
        else if (distanceToPlayer < detectionRange)
        {
            MoveVertically();
        }
        else
        {
            MoveToInitialPosition(); 
        }
    }

    void MoveVertically()
    {
        Vector3 targetPosition = new Vector3(transform.position.x, player.position.y, transform.position.z);
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
    }

    void MoveToInitialPosition()
    {
        transform.position = Vector3.MoveTowards(transform.position, _initialPosition, speed * Time.deltaTime);
    }

    void Attack()
    {
        Health playerHealth = player.GetComponent<Health>();
        if (playerHealth != null)
        {
            playerHealth.TakeDamage(attackDamage);

            if (playerHealth.currentHealth <= 0)
            {
                _isPlayerAlive = false;
            }
        }

        _lastAttackTime = Time.time;
        Debug.Log("Spider attacked the player!");
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && Time.time >= _lastAttackTime + attackCooldown)
        {
            Attack();
        }
    }
}
