using UnityEngine;
using UnityEngine.SceneManagement;

public class RespawnManager : MonoBehaviour
{
    public static RespawnManager Instance;

    private Transform lastCheckpointRespawnPoint; 
    private Transform benchRespawnPoint; 
    private string specificSceneName = "Room_1.1"; 

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

    public void SetCheckpointRespawnPoint(Vector3 checkpointPosition)
    {
        lastCheckpointRespawnPoint = new GameObject("LastCheckpoint").transform;
        lastCheckpointRespawnPoint.position = checkpointPosition;
    }

    public void SetBenchRespawnPoint(Transform benchTransform)
    {
        benchRespawnPoint = benchTransform;
    }

    public void RespawnPlayerAtBench()
    {
        if (benchRespawnPoint != null)
        {
            // Certifique-se de que a MasterScene permanece carregada
            SceneManager.LoadScene("MasterScene", LoadSceneMode.Additive); 
            SceneManager.sceneLoaded += OnSceneLoadedAtBench;
            // Carregar a cena específica sem descarregar a MasterScene
            SceneManager.LoadScene(specificSceneName); 
        }
        else
        {
            Debug.LogWarning("No bench respawn point set. Respawning at checkpoint.");
            RespawnPlayerAtCheckpoint();
        }
    }

    public void RespawnPlayerAtCheckpoint()
    {
        if (lastCheckpointRespawnPoint != null)
        {
            // Certifique-se de que a MasterScene permanece carregada
            SceneManager.LoadScene("MasterScene", LoadSceneMode.Additive);
            SceneManager.sceneLoaded += OnSceneLoadedAtCheckpoint;
            // Carregar a cena específica sem descarregar a MasterScene
            SceneManager.LoadScene(specificSceneName);
        }
        else
        {
            Debug.LogWarning("No checkpoint set. Cannot respawn.");
        }
    }

    private void OnSceneLoadedAtBench(Scene scene, LoadSceneMode mode)
    {
        GameObject player = Player.Instance.gameObject;
        PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();

        if (playerHealth != null)
        {
            player.SetActive(true);
            player.transform.position = benchRespawnPoint.position;
            playerHealth.RestoreHealth();
        }
        else
        {
            Debug.LogError("PlayerHealth script not found on Player object.");
        }

        SceneManager.sceneLoaded -= OnSceneLoadedAtBench;
    }

    private void OnSceneLoadedAtCheckpoint(Scene scene, LoadSceneMode mode)
    {
        GameObject player = Player.Instance.gameObject;
        PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();

        if (playerHealth != null)
        {
            player.SetActive(true);
            player.transform.position = lastCheckpointRespawnPoint.position;
            playerHealth.RestoreHealth();
        }
        else
        {
            Debug.LogError("PlayerHealth script not found on Player object.");
        }

        SceneManager.sceneLoaded -= OnSceneLoadedAtCheckpoint;
    }
}
