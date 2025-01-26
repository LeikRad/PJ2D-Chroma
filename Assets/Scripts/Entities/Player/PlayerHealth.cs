using Unity.VisualScripting;
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

    public float invulnerabilityTime = 0.4f; 
    private float invulnerabilityTimer = 0f; 
    private bool isInvulnerable = false;
    private GameObject[] enemies;

    public GameObject playerGameObject; // Reference to the player's GameObject

    public new void Start()
    {
        base.Start();
        player = GetComponent<Player>();
        damageFlash = GetComponent<DamageFlash>();
    }

    private void Update()
    {
        if (currentHealth > numOfHearts)
        {
            currentHealth = numOfHearts;
        }

        for (int i = 0; i < hearts.Length; i++)
        {
            if (i < currentHealth)
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
    }

    public override void TakeDamage(float amount, Transform attacker)
    {
        if (isInvulnerable)
        {
            return;
        }
        base.TakeDamage(amount);

        // Decrease health based on damage amount
        health -= Mathf.CeilToInt(amount);

        // Clamp health to ensure it stays within valid range
        health = Mathf.Clamp(health, 0, numOfHearts);

        enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemy in enemies)
        {
            Physics2D.IgnoreCollision(player.GetComponent<Collider2D>(), enemy.GetComponent<Collider2D>(),
                true);
        }
        Debug.Log($"Player took {amount} damage! Current health: {currentHealth}");
        isInvulnerable = true;
        invulnerabilityTimer = invulnerabilityTime; 

        if (attacker != null && currentHealth > 0)
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
        Player.Instance.GetComponent<Animator>().SetBool("Died", true);
        RespawnManager.Instance.RespawnPlayer();
    }
}