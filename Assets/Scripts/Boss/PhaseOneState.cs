using UnityEngine;
using System.Collections;

public class PhaseOneState : BossBaseState
{
    private float attackTimer;
    private float attackCooldown = 1.5f; 

    public PhaseOneState(BossStateMachine boss) : base(boss) { }

    public override void Enter()
    {
        Debug.Log("Entering Phase One");
        attackTimer = 0f;
       
    }

    public override void Update()
    {
        attackTimer += Time.deltaTime;

        if (attackTimer >= attackCooldown)
        {
            attackTimer = 0f;
            PerformSwoopAttack();
        }

        if (boss.Health < boss.MaxHealth * 0.66f)
        {
            boss.TransitionToState(boss.PhaseTwo);
        }
    }

    private void PerformSwoopAttack()
    {
        Debug.Log("Performing Swoop Attack");

        if (boss.SpiderLeg != null)
        {
            bool spawnFromRight = Random.Range(0, 2) == 0;
            float yPosition = boss.transform.position.y - 3f; 
            float startX = spawnFromRight ? 10f : -10f; 
            float endX = spawnFromRight ? -10f : 10f; 

            Vector3 startPosition = new Vector3(startX, yPosition, 0);
            Vector3 endPosition = new Vector3(endX, yPosition, 0);
            
            GameObject leg = GameObject.Instantiate(boss.SpiderLeg, startPosition, Quaternion.identity);
            leg.AddComponent<LegMovement>().Setup(startPosition, endPosition, 5f);
            Object.Destroy(leg, 5f);
        }
        else
        {
            Debug.LogWarning("LegPrefab not assigned in the BossStateMachine!");
        }
    }

    public override void Exit()
    {
        Debug.Log("Exiting Phase One");
    }
}