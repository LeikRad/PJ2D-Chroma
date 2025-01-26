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
        fade.fadeIn();

        GameObject player = GameObject.FindWithTag("Player");
        yield return new WaitForSeconds(fade.timeToFade);
        if (SceneManager.sceneCount > 1)
        {
            Scene oldScene = SceneManager.GetSceneAt(1);
            if (oldScene.IsValid() && oldScene.isLoaded)
            {
                Debug.Log("Descarregando cena: " + oldScene.name);
                yield return SceneManager.UnloadSceneAsync(oldScene);
            }
        }
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        if (player != null)
        {
            player.transform.position = position;
        }

        fade.fadeOut();
        yield return new WaitForSeconds(fade.timeToFade);
        SceneManager.SetActiveScene(SceneManager.GetSceneByName(sceneName));

        Debug.Log("Cena carregada: " + sceneName);
    }
}
