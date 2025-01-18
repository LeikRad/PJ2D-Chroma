using UnityEngine;

public class PhaseThreeState : BossBaseState
{
    private float platformSpawnTimer;
    private float platformSpawnInterval = 1.5f;
    private float lavaRiseSpeed = 1.0f;
    private float bossRiseSpeed = 0.6f;

    public PhaseThreeState(BossStateMachine boss) : base(boss) { }

    public override void Enter()
    {
        Debug.Log("Entering Phase Three");
        platformSpawnTimer = 0f;
        boss.Lava.gameObject.SetActive(true);
    }

    public override void Update()
    {
        platformSpawnTimer += Time.deltaTime;

        if (platformSpawnTimer >= platformSpawnInterval)
        {
            platformSpawnTimer = 0f;
            SpawnPlatform();
        }
        boss.Lava.position += Vector3.up * lavaRiseSpeed * Time.deltaTime;
        boss.transform.position += Vector3.up * bossRiseSpeed * Time.deltaTime; 
        if (boss.Health <= 0)
        {
            boss.TransitionToState(boss.Death);
        }
    }

    private void SpawnPlatform()
    {
        Debug.Log("Spawning Platform");
        float randomX = Random.Range(-7f, 7f); 
        Vector3 spawnPosition = new Vector3(randomX, boss.Lava.position.y + 3f, 0);
        GameObject.Instantiate(boss.PlatformPrefab, spawnPosition, Quaternion.identity);
    }

    public override void Exit()
    {
        Debug.Log("Exiting Phase Three");
    }
}

