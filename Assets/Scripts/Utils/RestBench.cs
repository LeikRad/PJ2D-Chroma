﻿using UnityEngine;
using UnityEngine.SceneManagement;

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
        Animator player_animator = Player.Instance.GetComponent<Animator>();
        if (Input.GetKeyDown(KeyCode.E) && Vector2.Distance(transform.position, Player.Instance.transform.position) < 2f)
        {
            SitAndRest();
            player_animator.SetBool("IsBench", true);
            Debug.Log("Player is resting");
        }
        else
        {
            player_animator.SetBool("IsBench", false);
        }
    }

    private void SitAndRest()
    {
        PlayerHealth playerHealth = Player.Instance.GetComponent<PlayerHealth>();
        playerHealth.RestoreHealth();
        RespawnManager.Instance.SetBenchRespawnPoint(benchSceneName, transform.position);
    }
}