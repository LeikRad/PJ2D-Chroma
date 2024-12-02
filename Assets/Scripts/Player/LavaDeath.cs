using UnityEngine;

public class LavaDeath : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // Call the player's death method
            collision.GetComponent<PlayerController>().Die();
        }
    }
}
