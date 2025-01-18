using UnityEngine;
using UnityEngine.SceneManagement;

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
                string currentScene = SceneManager.GetActiveScene().name;

                if (currentScene == "EnemyTest") 
                {
                    Debug.Log("Boss fight lava detected! Respawning player above lava.");
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
                    Debug.Log("Standard lava room detected! Using normal respawn system.");
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
            }
            else
            {
                Debug.LogWarning("PlayerHealth component is missing on the Player.");
            }
        }
    }
}
