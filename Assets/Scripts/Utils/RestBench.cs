using UnityEngine;

public class RestBench : MonoBehaviour
{
    public static Transform benchRespawnPoint; 

    private void Start()
    {
        benchRespawnPoint = transform;
        RespawnManager.Instance.SetBenchRespawnPoint(benchRespawnPoint); 
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
        PlayerHealth playerHealth = Player.Instance.GetComponent<PlayerHealth>();
        playerHealth.RestoreHealth();
        Debug.Log("Player is resting at the bench.");
    }
}
