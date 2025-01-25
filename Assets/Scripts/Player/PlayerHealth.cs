using Unity.VisualScripting;
using UnityEngine;

public class PlayerHealth : Health
{
    private Player player;
    public Animator animator;
    private DamageFlash damageFlash;

    public float invulnerabilityTime = 0.4f; 
    private float invulnerabilityTimer = 0f; 
    private bool isInvulnerable = false;
    private GameObject[] enemies;

    public new void Start()
    {
        base.Start();
        player = GetComponent<Player>();
        damageFlash = GetComponent<DamageFlash>();
    }

    private void Update()
    {
        if (isInvulnerable)
        {
            invulnerabilityTimer -= Time.deltaTime;
            if (invulnerabilityTimer <= 0)
            {
                isInvulnerable = false;
                if (enemies.Length > 0)
                {
                    //get all objects with tag enemy
                    foreach (GameObject enemy in enemies)
                    {
                        Physics2D.IgnoreCollision(player.GetComponent<Collider2D>(), enemy.GetComponent<Collider2D>(),
                            false);
                    }
                }
            }
        }

        if (currentHealth <= 0)
        {
            Player.Instance.GetComponent<Animator>().SetBool("Died", true);
        } else
        {
            Player.Instance.GetComponent<Animator>().SetBool("Died", false);
        }
    }

    public override void TakeDamage(float amount, Transform attacker)
    {
        if (isInvulnerable)
        {
            return;
        }
        base.TakeDamage(amount);
        enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemy in enemies)
        {
            Physics2D.IgnoreCollision(player.GetComponent<Collider2D>(), enemy.GetComponent<Collider2D>(),
                true);
        }
        Debug.Log($"Player took {amount} damage! Current health: {currentHealth}");
        isInvulnerable = true;
        invulnerabilityTimer = invulnerabilityTime; 

        if (attacker != null)
        {
            player.Knockback(attacker);
        }

        animator.SetBool("IsHurt", true);
        damageFlash.CallDamageFlash();
        animator.SetBool("IsHurt", false);


    }

    public void RestoreHealth()
    {
        currentHealth = maxHealth;
    }
    
    public void Death()
    {
        RespawnManager.Instance.RespawnPlayer();
    }
}