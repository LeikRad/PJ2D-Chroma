using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    private Transform currentRespawnPoint;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SetRespawnPoint(Transform respawnPoint)
    {
        currentRespawnPoint = respawnPoint;
    }

    public void RespawnPlayer()
    {
        if (currentRespawnPoint != null)
        {
            Player.Instance.gameObject.SetActive(true);
            Player.Instance.transform.position = currentRespawnPoint.position;
            Player.Instance.GetComponent<PlayerHealth>().RestoreHealth(100); 
            Debug.Log("Player respawned at the bench.");
        }
    }
}