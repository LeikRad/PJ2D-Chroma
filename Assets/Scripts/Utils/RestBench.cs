using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEditor;

public class RestBench : MonoBehaviour
{
    public string benchSceneName;

    private void Start()
    {
        Scene BenchScene = gameObject.scene;
        benchSceneName = BenchScene.name;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && Vector2.Distance(transform.position, Player.Instance.transform.position) < 2f)
        {
            Player.Instance.GetComponent<Animator>().SetBool("isBench", true);
            SitAndRest();
            StartCoroutine(Rest());
        }
    }

    private void SitAndRest()
    {
        PlayerHealth playerHealth = Player.Instance.GetComponent<PlayerHealth>();
        playerHealth.RestoreHealth();
        Player.Instance.canMove = false;
        RespawnManager.Instance.SetBenchRespawnPoint(benchSceneName, transform.position);
        SaveSystem.Save();
    }
    
    private IEnumerator Rest()
    {
        yield return new WaitForSeconds(1f);
        Player.Instance.canMove = true;
        Player.Instance.GetComponent<Animator>().SetBool("isBench", false);
    }
}