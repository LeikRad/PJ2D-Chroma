using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public BoxCollider2D trigger;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // get gameobject with tag respawn
            GameObject respawn = GameObject.FindGameObjectWithTag("Respawn");
            // for each child in respawn update the position to the current checkpoint
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