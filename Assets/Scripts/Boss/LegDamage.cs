using UnityEngine;

public class LegDamage : MonoBehaviour
{
    public int damage = 10; 

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player")) 
        {
            Debug.Log("Player hit by leg!");
            PlayerHealth player = collision.gameObject.GetComponent<PlayerHealth>();
            if (player != null)
            {
                player.TakeDamage(damage, null);
            }
        }
    }
}