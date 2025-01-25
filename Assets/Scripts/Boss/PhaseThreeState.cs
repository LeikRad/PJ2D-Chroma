using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PhaseThreeState : BossBaseState
{
    private float attackTimer;
    private float attackCooldown = 2.5f; // Faster attack rate
    private int legsToSpawn = 6;
    private List<Vector3> usedPositions = new List<Vector3>();
    private float minSpacing = 2f;

    public PhaseThreeState(BossStateMachine boss) : base(boss) { }

    public override void Enter()
    {
        Debug.Log("Entering Phase Three - Enraged Mode");
        attackTimer = 0f;
    }

    public override void Update()
    {
        attackTimer += Time.deltaTime;

        if (attackTimer >= attackCooldown)
        {
            attackTimer = 0f;
            PerformLegAttacks();
            PerformSwoopAttack();
        }

        if (boss.Health <= 100)
        {
            boss.IsInvulnerable = true;
            boss.TransitionToState(boss.PhaseFour);
        }
    }

    private void PerformLegAttacks()
    {
        Debug.Log("Performing Enraged Leg Attacks");
        usedPositions.Clear();

        int spawnedLegs = 0;
        while (spawnedLegs < legsToSpawn)
        {
            float randomX = Random.Range(-9f, 9f);
            Vector3 attackPosition = new Vector3(randomX, -3, 0);
            bool tooClose = false;
            foreach (Vector3 pos in usedPositions)
            {
                if (Mathf.Abs(pos.x - attackPosition.x) < minSpacing)
                {
                    tooClose = true;
                    break;
                }
            }

            if (!tooClose)
            {
                usedPositions.Add(attackPosition);
                StartCoroutine(WarningAndAttack(attackPosition));
                spawnedLegs++;
            }
        }
    }

    private void PerformSwoopAttack()
    {
        Debug.Log("Performing Enraged Swoop Attack");

        if (boss.SpiderLeg != null)
        {
            bool spawnFromRight = Random.Range(0, 2) == 0;
            float yPosition = boss.transform.position.y - 3f;
            float startX = spawnFromRight ? 10f : -10f;
            float endX = spawnFromRight ? -10f : 10f;

            Vector3 startPosition = new Vector3(startX, yPosition, 0);
            Vector3 endPosition = new Vector3(endX, yPosition, 0);
            
            GameObject leg = GameObject.Instantiate(boss.SpiderLeg, startPosition, Quaternion.identity);
            leg.AddComponent<LegMovement>().Setup(startPosition, endPosition, 7f);
            Object.Destroy(leg, 4f);
        }
        else
        {
            Debug.LogWarning("LegPrefab not assigned in the BossStateMachine!");
        }
    }
    
    private IEnumerator WarningAndAttack(Vector3 position)
    {
        GameObject warning = GameObject.Instantiate(boss.GroundLegPrefab, position, Quaternion.identity);
        SpriteRenderer sr = warning.GetComponent<SpriteRenderer>();
        sr.color = new Color(1f, 0f, 0f, 0.5f);
        Collider2D warningCollider = warning.GetComponent<Collider2D>();
        if (warningCollider != null) warningCollider.enabled = false; 
        
        yield return new WaitForSeconds(1f); 
        
        GameObject.Destroy(warning);
        GameObject leg = GameObject.Instantiate(boss.GroundLegPrefab, position, Quaternion.identity);
        SpriteRenderer legSR = leg.GetComponent<SpriteRenderer>();
        legSR.color = Color.white;
        leg.transform.localScale = new Vector3(1f, 2f, 1f); 

        yield return new WaitForSeconds(0.5f);
        
        Collider2D hitPlayer = Physics2D.OverlapBox(position, new Vector2(1f, 2f), 0);
        if (hitPlayer != null && hitPlayer.CompareTag("Player"))
        {
            PlayerHealth playerHealth = hitPlayer.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(20, null);
            }
        }
        GameObject.Destroy(leg, 0.5f);
    }
    
    public override void Exit()
    {
        Debug.Log("Exiting Phase Three - Enraged Mode");
    }
}