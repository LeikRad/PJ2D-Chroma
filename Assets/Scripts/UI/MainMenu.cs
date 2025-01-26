using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void GoToScene(string sceneName)
    {
        SceneChanger.Instance.ChangeScene(sceneName, new Vector3(0f, 0f, 0f));
        // SceneManager.LoadScene(sceneName);
    }

    public void GoToOptions()
    {
        SceneManager.LoadScene("OptionsMenu");
        //SceneManager.UnloadScene("MasterScene");
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Game is exiting");
    }
}
