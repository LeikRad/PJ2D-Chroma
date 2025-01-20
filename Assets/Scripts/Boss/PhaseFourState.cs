using UnityEngine;

public class PhaseFourState : BossBaseState
{
    private float bossSpeed = 4f;
    private bool playerReachedFinalPlatform = false;
    private PlayerHealth playerHealth;
    private string benchSceneName;
    private Vector3 benchRespawnPosition;

    public PhaseFourState(BossStateMachine boss) : base(boss) { }

    public override void Enter()
    {
        Debug.Log("Entering Final Phase - Ultra Fast Mode");
        boss.IsInvulnerable = true;
        boss.PhaseFourStartPosition = boss.transform.position;
        boss.Health = 1; 
        SpawnPlatformsToFinalPlatform();
        playerHealth = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHealth>();
    }

    public override void Update()
    {
        if (!playerReachedFinalPlatform)
        {
            boss.transform.position += Vector3.up * bossSpeed * Time.deltaTime;
            boss.Lava.position += Vector3.up * boss.LavaRiseSpeed * Time.deltaTime; // Lava rises slower
        }
        
        if (boss.BossStopPoint != null && boss.transform.position.y >= boss.BossStopPoint.position.y)
        {
            boss.transform.position = new Vector3(boss.transform.position.x, boss.BossStopPoint.position.y, boss.transform.position.z);
        }
        {
            boss.transform.position = new Vector3(boss.transform.position.x, boss.BossStopPoint.position.y, boss.transform.position.z);
        }
        
        if (boss.FinalPlatform != null && Vector3.Distance(GameObject.FindGameObjectWithTag("Player").transform.position, boss.FinalPlatform.transform.position) < 1f)
        {
            playerReachedFinalPlatform = true;
            boss.IsInvulnerable = false;
        }
    }

    private void SpawnPlatformsToFinalPlatform()
    {
        Vector3 spawnPosition = boss.PlatformSpawnPoint.position;
        while (spawnPosition.y < boss.FinalPlatform.transform.position.y - boss.BossFinalHeightOffset)
        {
            float horizontalOffset = Random.Range(-boss.PlatformHorizontalVariation, boss.PlatformHorizontalVariation);
            Vector3 newPosition = new Vector3(spawnPosition.x + horizontalOffset, spawnPosition.y, spawnPosition.z);
            GameObject.Instantiate(boss.PlatformPrefab, newPosition, Quaternion.identity);
            spawnPosition.y += boss.PlatformSpacing;
        }
    }
    
    public override void Exit()
    {
        Debug.Log("Exiting Final Phase");
    }
}
