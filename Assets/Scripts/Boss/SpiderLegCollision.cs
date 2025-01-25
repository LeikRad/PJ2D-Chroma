using UnityEngine;

public class SpiderLegCollision : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerHealth player = collision.GetComponent<PlayerHealth>();
            if (player != null)
            {
                player.TakeDamage(10); 
                if (player.GetHealth() <= 0)
                {
                    player.Death();
                }
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        Debug.Log("Collision detected");
    }
}