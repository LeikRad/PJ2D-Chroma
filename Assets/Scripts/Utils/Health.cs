using UnityEngine;
using System.Collections;

public class Health : MonoBehaviour
{
    public float maxHealth = 100f;
    protected float currentHealth;
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

    public void TakeDamage(float amount)
    {

        currentHealth -= amount;

        if (currentHealth <= 0 && CompareTag("Enemy"))
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
}
