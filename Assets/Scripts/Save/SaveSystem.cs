using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SaveSystem
{
    private static SaveData _saveData = new SaveData();
    
    [System.Serializable]
    public struct SaveData
    {
        public Vector3 PlayerPosition;
        public float PlayerHealth;
        public bool HasWeapon;
        public string CurrentScene;
        public bool BossDefeated;
        public bool PlatformsActivated;
        public bool LavaDestroyed;
        public string BenchSceneName;  
        public Vector3 BenchRespawnPosition;
        public int MaxJumps;
        public bool DashEnabled;
    }
    
    public static string SaveFilename()
    {
        return Application.persistentDataPath + "/savegame.json";
    }

    public static void Save()
    {
        HandleSaveData();
        File.WriteAllText(SaveFilename(), JsonUtility.ToJson(_saveData, true));
    }
    
    private static void HandleSaveData()
    {
        PlayerStateMachine player = GameObject.FindObjectOfType<PlayerStateMachine>();
        if (player == null)
        {
            Debug.LogError("PlayerStateMachine não encontrado!");
            return;
        }
        _saveData.PlayerPosition = player.transform.position;
        _saveData.MaxJumps = player.maxJumps;
        _saveData.DashEnabled = player.dashEnabled;
        
        PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();
        if (playerHealth != null)
        {
            _saveData.PlayerHealth = playerHealth.currentHealth;
        }
        
        PlayerWeapon playerWeapon = player.GetComponent<PlayerWeapon>();
        if (playerWeapon != null)
        {
            _saveData.HasWeapon = playerWeapon.equippedWeapon != null;
        }
        
        _saveData.CurrentScene = SceneManager.GetSceneAt(1).name;
        _saveData.BenchSceneName = RespawnManager.Instance.benchSceneName;
        _saveData.BenchRespawnPosition = RespawnManager.Instance.GetBenchRespawnPosition();
    }
    
    public static void Load()
    {
        if (!File.Exists(SaveFilename()))
        {
            Debug.LogError("Nenhum arquivo de save encontrado!");
            return;
        }

        string saveContent = File.ReadAllText(SaveFilename());
        _saveData = JsonUtility.FromJson<SaveData>(saveContent);
        GameManager.Instance.StartCoroutine(LoadSavedScene());
    }
    
    public static IEnumerator LoadSavedScene()
    {
        if (string.IsNullOrEmpty(_saveData.CurrentScene))
        {
            Debug.LogError("Nenhuma cena salva!");
            yield break;
        }
        
        Scene masterScene = SceneManager.GetSceneAt(0);
        if (!masterScene.isLoaded)
        {
            yield return SceneManager.LoadSceneAsync("MasterScene", LoadSceneMode.Single);
        }
        
        if (SceneManager.sceneCount > 1)
        {
            Scene oldScene = SceneManager.GetSceneAt(1);
            if (oldScene.isLoaded)
            {
                yield return SceneManager.UnloadSceneAsync(oldScene);
            }
        }
        
        yield return SceneManager.LoadSceneAsync(_saveData.CurrentScene, LoadSceneMode.Additive);
        SceneManager.SetActiveScene(SceneManager.GetSceneByName(_saveData.CurrentScene));
        HandleLoadData();
    }
    
    private static void HandleLoadData()
    {
        PlayerStateMachine player = GameObject.FindObjectOfType<PlayerStateMachine>();
        if (player == null)
        {
            Debug.LogError("PlayerStateMachine não encontrado!");
            return;
        }
        
        player.transform.position = _saveData.PlayerPosition;
        player.maxJumps = _saveData.MaxJumps;
        player.dashEnabled = _saveData.DashEnabled;
        
        PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();
        if (playerHealth != null)
        {
            playerHealth.currentHealth = _saveData.PlayerHealth;
        }
        
        PlayerWeapon playerWeapon = player.GetComponent<PlayerWeapon>();
        if (playerWeapon != null && _saveData.HasWeapon)
        {
            playerWeapon.EquipDefaultWeapon();
        }
        
        if (_saveData.BossDefeated)
        {
            GameManager.Instance.DestroyBossAndLava();
        }
        if (!string.IsNullOrEmpty(_saveData.BenchSceneName))
        {
            RespawnManager.Instance.SetBenchRespawnPoint(_saveData.BenchSceneName, _saveData.BenchRespawnPosition);
        }
    }

    public static void MarkBossAsDefeated()
    {
        _saveData.BossDefeated = true;
        _saveData.LavaDestroyed = true; 
        Save();
    }

    public static bool IsBossDefeated()
    {
        return _saveData.BossDefeated;
    }

    public static bool IsLavaDestroyed()
    {
        return _saveData.LavaDestroyed;
    }
}