using UnityEngine;


[RequireComponent(typeof(Collider2D))]
public class LevelChange : MonoBehaviour
{
    public string sceneName;
    public Vector3 playerPosition;   
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            SceneChanger.Instance.ChangeScene(sceneName, playerPosition);
        }
    }
}
