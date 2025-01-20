using UnityEngine;
using System.Collections;

public class Health : MonoBehaviour
{
    public float maxHealth = 100f;
    public float currentHealth;
    public float invulnerabilityDuration = 3f; 
    private bool isInvulnerable = false; 
    private Collider2D objectCollider;

    public void Start()
    {
        currentHealth = maxHealth;
        objectCollider = GetComponent<Collider2D>();

        if (objectCollider == null)
        {
            Debug.LogWarning("No Collider2D attached to the GameObject.");
        }

        Animator play_animator = Player.Instance.GetComponent<Animator>();
        play_animator.SetBool("Died", false);
    }

    public virtual void TakeDamage(float amount)
    {
        if (isInvulnerable) return; 

        currentHealth -= amount;

        if (currentHealth <= 0)
        {
            Die();

        }
        else
        {
            StartCoroutine(InvulnerabilityTimer());
        }
    }

    public virtual void TakeDamage(float amount, Transform attacker)
    {
        TakeDamage(amount);
    }

    void Die()
    {
        //gameObject.SetActive(false);
        Debug.Log("Player is Dead!");

        Animator play_animator = Player.Instance.GetComponent<Animator>();
        play_animator.SetBool("Died", true);
    }

    public float GetHealth()
    {
        return currentHealth;
    }

    private IEnumerator InvulnerabilityTimer()
    {
        isInvulnerable = true;
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemy in enemies)
        {
            Collider2D enemyCollider = enemy.GetComponent<Collider2D>();
            if (enemyCollider != null && objectCollider != null)
            {
                Physics2D.IgnoreCollision(objectCollider, enemyCollider, true);
            }
        }

        yield return new WaitForSeconds(invulnerabilityDuration);
        
        foreach (GameObject enemy in enemies)
        {
            Collider2D enemyCollider = enemy.GetComponent<Collider2D>();
            if (enemyCollider != null && objectCollider != null)
            {
                Physics2D.IgnoreCollision(objectCollider, enemyCollider, false);
            }
        }

        isInvulnerable = false;
    }
}
