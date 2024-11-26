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
        Debug.Log("Changing Scene to: " + sceneName);
        /*
        StartCoroutine(LoadScene(sceneName, position));
        */
        
    }
    /*
private IEnumerator LoadScene(string sceneName, Vector3 position)
{
    // disable player and camera
    player.SetActive(false);

    // load new scene
    yield return SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);

    // set player position
    player.transform.position = position;
    Camera.transform.position = new Vector3(position.x, position.y, -10);
    // enable player and camera
    player.SetActive(true);

    // unload current scene
    yield return SceneManager.UnloadSceneAsync(currentScene);

    // update current scene
    currentScene = SceneManager.GetSceneByName(sceneName);
        
    }    */

}
