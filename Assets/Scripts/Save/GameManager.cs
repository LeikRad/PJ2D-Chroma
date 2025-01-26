using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SaveGame()
    {
        SaveSystem.Save();
        Debug.Log("Jogo salvo!");
    }

    public void LoadGame()
    {
        SaveSystem.Load();
    }
    
    public void DestroyBossAndLava()
    {
        var boss = GameObject.FindObjectOfType<BossStateMachine>(); 
        if (boss != null)
        {
            Destroy(boss.gameObject);
        }

        var lava = GameObject.FindWithTag("BossLava"); 
        if (lava != null)
        {
            Destroy(lava);
        }
    }
}