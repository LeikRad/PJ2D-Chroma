using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PhaseTwoState : BossBaseState
{
    private float attackTimer;
    private float attackCooldown = 3f;
    private int legsToSpawn = 4;
    private List<Vector3> usedPositions = new List<Vector3>();
    private float minSpacing = 2f; 

    public PhaseTwoState(BossStateMachine boss) : base(boss) { }

    public override void Enter()
    {
        Debug.Log("Entering Phase Two");
        attackTimer = 0f;
    }

    public override void Update()
    {
        attackTimer += Time.deltaTime;

        if (attackTimer >= attackCooldown)
        {
            attackTimer = 0f;
            PerformLegAttacks();
        }

        if (boss.Health < boss.MaxHealth * 0.5f)
        {
            boss.TransitionToState(boss.PhaseThree);
        }
    }

    private void PerformLegAttacks()
    {
        Debug.Log("Performing Leg Attacks");
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
        Debug.Log("Exiting Phase Two");
    }
}

