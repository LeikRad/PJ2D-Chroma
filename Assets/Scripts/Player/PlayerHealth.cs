using UnityEngine;

public class PlayerHealth : Health
{
    private Player player;
    public Animator animator;
    private DamageFlash damageFlash;

    public float invulnerabilityTime = 1.5f; 
    private float invulnerabilityTimer = 0f; 
    private bool isInvulnerable = false;

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
            }
        }
    }

    public override void TakeDamage(float amount, Transform attacker)
    {
        if (isInvulnerable)
        {
            return;
        }

        base.TakeDamage(amount);
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

        if (currentHealth <= 0)
        {
            Debug.Log("Player died!");
            Death();
        }
    }

    public void RestoreHealth()
    {
        currentHealth = maxHealth;
    }
    
    public void Death()
    {
        RespawnManager.Instance.RespawnPlayerAtBench();
    }
}