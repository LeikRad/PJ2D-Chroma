using UnityEngine;

public class RespawnController : MonoBehaviour
{
    public float attackDamage = 10f;
    public Transform respawnPoint; 

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerHealth playerHealth = collision.gameObject.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(attackDamage, null);
                RespawnManager.Instance.SetCheckpointRespawnPoint(respawnPoint.position);
                if (playerHealth.GetHealth() > 0)
                {
                    collision.transform.position = respawnPoint.position;
                }
                else
                {
                    RespawnManager.Instance.RespawnPlayerAtBench();
                }
            }
            else
            {
                Debug.LogWarning("PlayerHealth component is missing on the Player.");
            }
        }
    }
}