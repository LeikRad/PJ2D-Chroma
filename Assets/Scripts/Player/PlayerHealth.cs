using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : Health
{
    public int health;
    public int numOfHearts;

    public Image[] hearts;
    public Sprite fullHeart;
    public Sprite emptyHeart;

    private Player player;
    public Animator animator;
    private DamageFlash damageFlash;

    public GameObject playerGameObject; // Reference to the player's GameObject

    public new void Start()
    {
        base.Start();
        player = GetComponent<Player>();
        damageFlash = GetComponent<DamageFlash>();
    }

    public override void TakeDamage(float amount, Transform attacker)
    {
        base.TakeDamage(amount);

        // Decrease health based on damage amount
        health -= Mathf.CeilToInt(amount);

        // Clamp health to ensure it stays within valid range
        health = Mathf.Clamp(health, 0, numOfHearts);

        if (attacker != null)
        {
            player.Knockback(attacker);
        }
        animator.SetBool("IsHurt", true);
        damageFlash.CallDamageFlash();
        animator.SetBool("IsHurt", false);
    }

    void Update(){
        if (health > numOfHearts)
        {
            health = numOfHearts;
        }

        for (int i = 0; i < hearts.Length; i++)
        {
            if (i < health)
            {
                hearts[i].sprite = fullHeart;
            }
            else
            {
                hearts[i].sprite = emptyHeart;
            }

            if (i < numOfHearts)
            {
                hearts[i].enabled = true;
            }
            else
            {
                hearts[i].enabled = false;
            }
        }

        if (health <= 0)
        {
            playerGameObject.SetActive(false);
        }
    }
}