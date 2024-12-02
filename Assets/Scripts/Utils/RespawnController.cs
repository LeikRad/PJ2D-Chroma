using UnityEngine;
 
public class RespawnController : MonoBehaviour
{
    public float attackDamage = 10f;
    private bool isPlayerAlive = true;
    public static RespawnController Instance;
 
    public Transform respawnPoint;
 
    private void Awake()
    {
        Instance = this;
    }
 
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
            Debug.Log("Player entered the respawn point");
        {
            Health playerHealth = collision.gameObject.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(attackDamage, null);

                if (playerHealth.GetHealth() <= 0)
                {
                    isPlayerAlive = false;
                }
            }
            collision.transform.position = respawnPoint.position;
        }
    }
}