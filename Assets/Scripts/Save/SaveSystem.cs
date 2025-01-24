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
        Player player = Player.Instance;
        _saveData.PlayerPosition = player.transform.position;
        _saveData.PlayerHealth = player.GetComponent<PlayerHealth>().currentHealth;
        _saveData.HasWeapon = player.GetComponent<PlayerWeapon>().equippedWeapon != null;
        _saveData.CurrentScene = SceneManager.GetSceneAt(1).name;
    }
    
    public static void Load()
    {
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
        Player player = Player.Instance;
        PlayerWeapon playerWeapon = player.GetComponent<PlayerWeapon>();
        if (playerWeapon == null)
        {
            return;
        }
        player.transform.position = _saveData.PlayerPosition;
        player.GetComponent<PlayerHealth>().currentHealth = _saveData.PlayerHealth;

        if (_saveData.HasWeapon)
        {
            player.GetComponent<PlayerWeapon>().EquipDefaultWeapon();
        }
        if (_saveData.BossDefeated)
        {
            GameManager.Instance.DestroyBossAndLava();
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
