using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RespawnManager : MonoBehaviour
{
    public static RespawnManager Instance;
    public string benchSceneName;
    public static Vector3 benchRespawnPosition;
    private Scene currentScene;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void SetBenchRespawnPoint(string sceneName, Vector3 position)
    {
        benchSceneName = sceneName;
        benchRespawnPosition = position;
    }
    public void RespawnPlayer()
    {
        if (!string.IsNullOrEmpty(benchSceneName))
        {
            currentScene = SceneManager.GetSceneAt(1);
            SceneManager.UnloadSceneAsync(currentScene);
            SceneManager.LoadScene(benchSceneName, LoadSceneMode.Additive);
            Player.Instance.transform.position = benchRespawnPosition;
            currentScene = SceneManager.GetSceneByName(benchSceneName);
        }
        else
        {
            Debug.LogWarning("No bench scene name set. Could not respawn.");
        }
    }
}