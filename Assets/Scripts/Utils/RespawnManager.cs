﻿using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RespawnManager : MonoBehaviour
{
    public static RespawnManager Instance;
    public string benchSceneName;
    public static Vector3 benchRespawnPosition;
    private Scene thisScene;
    public Scene currentScene;

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
    
    public Vector3 GetBenchRespawnPosition()
    {
        return benchRespawnPosition;
    }
    public void RespawnPlayer()
    {
        if (!string.IsNullOrEmpty(benchSceneName))
        {
            Player.Instance.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
            StartCoroutine(WaitAndRespawn());
        }
        else
        {
            Debug.LogError("Bench respawn point not set!");
            Application.Quit();
        }
    }
    
    private IEnumerator WaitAndRespawn()
    {
        yield return new WaitForSeconds(1.5f);
        currentScene = SceneChanger.Instance.currentScene;
        thisScene = SceneManager.GetSceneAt(1);
        SceneManager.UnloadSceneAsync(thisScene);
        SceneManager.LoadSceneAsync(benchSceneName, LoadSceneMode.Additive);
        Player.Instance.transform.position = benchRespawnPosition;
        PlayerHealth playerHealth = Player.Instance.GetComponent<PlayerHealth>();
        playerHealth.RestoreHealth();
        SceneChanger.Instance.SetCurrentScene(SceneManager.GetSceneByName(benchSceneName));
        Player.Instance.GetComponent<Animator>().SetBool("Died", false);
        Player.Instance.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;
        Player.Instance.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
    }
}