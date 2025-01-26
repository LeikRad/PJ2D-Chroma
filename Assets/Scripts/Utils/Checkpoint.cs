using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public BoxCollider2D trigger;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            GameObject respawn = GameObject.FindGameObjectWithTag("Respawn");
            foreach (Transform child in respawn.transform)
            {
                RespawnController respawnController = child.GetComponent<RespawnController>();
                respawnController.respawnPoint = transform;
            }
            trigger.enabled = false;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            trigger.enabled = true;
        }
    }
}