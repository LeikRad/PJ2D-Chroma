using UnityEngine;
using UnityEngine.SceneManagement;

public class RestBench : MonoBehaviour
{
    public string benchSceneName;

    private void Start()
    {
        Scene BenchScene = gameObject.scene;
        benchSceneName = BenchScene.name;
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
        RespawnManager.Instance.SetBenchRespawnPoint(benchSceneName, transform.position);
    }
}
