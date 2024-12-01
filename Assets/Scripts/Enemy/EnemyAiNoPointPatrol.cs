using UnityEngine;

public class EnemyAiNoPointPatrol : MonoBehaviour
{
    public float patrolDistance = 2f; // Distância máxima que o inimigo pode se mover para os lados
    public float speed = 2f;         // Velocidade de movimento
    public Transform player;         // Referência ao jogador
    public float detectionRange = 10f; // Distância para detectar o jogador
    public float attackDamage = 10f;
    private Vector3 spawnPosition;   // Posição inicial onde o inimigo foi instanciado
    private bool movingRight = true; // Indica se o inimigo está indo para a direita
    private bool isChasingPlayer = false; // Indica se o inimigo está perseguindo o jogador
    private bool isPlayerAlive = true;

    void Start()
    {
        spawnPosition = transform.position; // Salva a posição inicial do inimigo
    }

    void Update()
    {
        if (!isPlayerAlive || IsPlayerInRange())
        {
            ChasePlayer();
        }
        else
        {
            Patrol();
        }
    }

    void Patrol()
    {
        // Calcula as bordas do movimento permitido
        float leftBoundary = spawnPosition.x - patrolDistance;
        float rightBoundary = spawnPosition.x + patrolDistance;

        // Atualiza a posição do inimigo
        if (movingRight)
        {
            transform.Translate(Vector3.right * speed * Time.deltaTime);
            if (transform.position.x >= rightBoundary)
            {
                movingRight = false;
            }
        }
        else
        {
            transform.Translate(Vector3.left * speed * Time.deltaTime);
            if (transform.position.x <= leftBoundary)
            {
                movingRight = true;
            }
        }
    }

    void ChasePlayer()
    {
        // Movimenta-se em direção ao jogador
        Vector3 direction = (player.position - transform.position).normalized;
        transform.Translate(direction * speed * Time.deltaTime);
    }

    bool IsPlayerInRange()
    {
        // Verifica se o jogador está dentro do alcance
        float distanceToPlayer = Vector3.Distance(player.position, transform.position);
        bool inRange = distanceToPlayer <= detectionRange;
        return inRange;
    }
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
