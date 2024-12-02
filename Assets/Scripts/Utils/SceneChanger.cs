using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneChanger : MonoBehaviour
{
    public static SceneChanger Instance { get; private set; }
    
    private Scene currentScene;
    
    private void Awake()
    {
        if (Instance == null) Instance = this;
    }
    
    private void Start()
    {
        currentScene = SceneManager.GetSceneAt(1);
        Debug.Log("Current Scene: " + currentScene.name);
    }
    
    public void ChangeScene(string sceneName, Vector3 position)
    {
        StartCoroutine(LoadScene(sceneName, position));
    }
    
    private IEnumerator LoadScene(string sceneName, Vector3 position)
    {
        // disable player and camera

        // unload current scene
        yield return SceneManager.UnloadSceneAsync(currentScene);
        
        // load next scene
        yield return SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
        // get player object and move it
        GameObject player = GameObject.FindWithTag("Player");
        player.transform.position = position;
        
        // update current scene
        currentScene = SceneManager.GetSceneByName(sceneName);
    }
}
