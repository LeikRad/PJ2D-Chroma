using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 10f; // Velocidade da bala
    public int damage = 10;   // Dano da bala
    public float maxDistance = 10f; 
    private Vector3 startPosition;  // Posição inicial da bala
    private void Start()
    {
        // Define a direção da bala
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        Vector2 direction = transform.right; // A direção para a qual a arma está apontando
        rb.linearVelocity = direction * speed; // Movimento da bala
        startPosition = transform.position;
    }
    
    void Update()
    {
        // Verifica se a bala percorreu a distância máxima
        if (Vector3.Distance(startPosition, transform.position) >= maxDistance)
        {
            Destroy(gameObject);  // Destroi a bala após a distância máxima
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // Detecta colisões com inimigos ou outros objetos
        if (collision.gameObject.CompareTag("Enemy"))
        {
            //collision.gameObject.GetComponent<EnemyHealth>().TakeDamage(damage); // Dano no inimigo
            Destroy(gameObject); // Destroi a bala após a colisão
        }
        else
        {
            Destroy(gameObject); // Destroi a bala se colidir com outros objetos
        }
    }
}