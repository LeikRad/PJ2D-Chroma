using UnityEngine;

public class RespawnController : MonoBehaviour
{
    public float attackDamage = 10f;
    private bool isPlayerAlive = true;
    public Transform respawnPoint;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
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
            Debug.Log(respawnPoint.position);
            collision.transform.position = respawnPoint.position;

            //if (isPlayerAlive)
            //{
                //animator.setBoolean("isDead", false);
            //}
            //else
            //{
                //animator.setBoolean("isDead", true);
            //}
        }
    }
}