using UnityEngine;


public class PlayerHealth : Health
{
    private Player player;
    
    public new void Start()
    {
        base.Start();
        player = GetComponent<Player>();
    }

    public override void TakeDamage(float amount, Transform  attacker)
    {
        base.TakeDamage(amount);
        player.Knockback(attacker);
    }
}