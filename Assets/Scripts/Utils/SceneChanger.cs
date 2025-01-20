using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneChanger : MonoBehaviour
{
    public static SceneChanger Instance { get; private set; }
    
    public Scene currentScene;
    private FadeInOut fade;
    private void Awake()
    {
        if (Instance == null) Instance = this;
    }
    
    private void Start()
    {
        // get fade script inside same object
        fade = GetComponent<FadeInOut>();
        // load main menu scene
        SceneManager.LoadScene("MainMenu", LoadSceneMode.Additive);
        currentScene = SceneManager.GetSceneAt(1);
        Debug.Log("Current Scene: " + currentScene.name);
    }
    
    public void SetCurrentScene(Scene scene)
    {
        currentScene = scene;
    }
    
    public void ChangeScene(string sceneName, Vector3 position)
    {
        StartCoroutine(LoadScene(sceneName, position));
    }
    
    private IEnumerator LoadScene(string sceneName, Vector3 position)
    {
        // FdaeIn
        fade.fadeIn();
        // disable player movement
        GameObject player = GameObject.FindWithTag("Player");
        yield return new WaitForSeconds(fade.timeToFade);
        // unload current scene
        yield return SceneManager.UnloadSceneAsync(currentScene);
        
        // load next scene
        yield return SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
        // get player object and move it
        if (player != null && position != Vector3.zero)
        {
            player.transform.position = position;
        }
        // FadeOut
        fade.fadeOut();
        yield return new WaitForSeconds(fade.timeToFade);
        
        //enable player movement
        
        // update current scene
        currentScene = SceneManager.GetSceneByName(sceneName);
    }
}
