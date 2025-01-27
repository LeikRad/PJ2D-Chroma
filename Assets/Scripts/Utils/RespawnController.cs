using UnityEngine;

public class RespawnController : MonoBehaviour
{
    public float attackDamage = 1f; 
    public Transform respawnPoint;
    private BossStateMachine boss;

    private void Awake()
    {
        boss = FindObjectOfType<BossStateMachine>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) 
        {
            PlayerHealth playerHealth = collision.gameObject.GetComponent<PlayerHealth>();
            
            playerHealth.TakeDamage(attackDamage, transform);
            Debug.Log("Player hit by lava!");

            if (gameObject.CompareTag("BossLava")) 
            {
                if (boss != null)
                {
                    boss.ResetPhaseFour();
                }
            }
            if (playerHealth.GetHealth() > 0)
            {
                if (respawnPoint != null)
                {
                    collision.transform.position = respawnPoint.position; 
                }
            }
            else
            {
                playerHealth.Death();
            }
        }
    }
}