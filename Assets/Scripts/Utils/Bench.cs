using UnityEngine;

public class RestBench : MonoBehaviour
{
    public float healthRecoveryAmount = 30f; 
    public static Transform benchRespawnPoint; 

    private void Start()
    {
        // Define o ponto inicial do banco como o ponto de respawn
        benchRespawnPoint = transform;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && Vector2.Distance(transform.position, Player.Instance.transform.position) < 2f)
        {
            SitAndRest();
        }
    }

    private void SitAndRest()
    {
        // Recuperar saúde do jogador
        PlayerHealth playerHealth = Player.Instance.GetComponent<PlayerHealth>();
        if (playerHealth != null)
        {
            playerHealth.RestoreHealth(healthRecoveryAmount);
        }

        // Define o banco como ponto de respawn ativo
        GameManager.Instance.SetRespawnPoint(transform);

        Debug.Log("Player is resting at the bench.");
    }
}