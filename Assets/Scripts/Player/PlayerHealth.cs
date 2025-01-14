using UnityEngine;


public class PlayerHealth : Health
{
    private Player player;
    public Animator animator;
    private DamageFlash damageFlash;

    public new void Start()
    {
        base.Start();
        player = GetComponent<Player>();
        damageFlash = GetComponent<DamageFlash>();
    }

    public override void TakeDamage(float amount, Transform attacker)
    {
        base.TakeDamage(amount);
        if (attacker != null)
        {
            player.Knockback(attacker);
        }
        animator.SetBool("IsHurt", true);
        damageFlash.CallDamageFlash();
        animator.SetBool("IsHurt", false);
    }
    
    public void RestoreHealth(float amount)
    {
        currentHealth = Mathf.Min(currentHealth + amount, maxHealth);
        Debug.Log("Health restored to: " + currentHealth);
    }
}