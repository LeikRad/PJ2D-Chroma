using UnityEngine;

public class RespawnController : MonoBehaviour
{
    public float attackDamage = 10f;
    public Transform respawnPoint; // Ponto de respawn para a área específica (lava)

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerHealth playerHealth = collision.gameObject.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                // Aplica dano ao jogador
                playerHealth.TakeDamage(attackDamage, null);

                // Teleporta o jogador para o ponto de respawn da área (não altera o ponto de respawn geral)
                collision.transform.position = respawnPoint.position;

                // Se a saúde chegar a 0, o jogador morre e renasce no ponto geral
                if (playerHealth.GetHealth() <= 0)
                {
                    GameManager.Instance.RespawnPlayer();
                }
            }
        }
    }
}