using UnityEngine;
using System.Collections;

public class Health : MonoBehaviour
{
    public float maxHealth = 100f;
    protected float currentHealth;

    public void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(float amount)
    {

        currentHealth -= amount;

        if (currentHealth <= 0 && CompareTag("Enemy"))
        {
            Die();
        }
    }

    public virtual void TakeDamage(float amount, Transform attacker)
    {
        TakeDamage(amount);
    }

    void Die()
    {
        gameObject.SetActive(false);
    }

    public float GetHealth()
    {
        return currentHealth;
    }
}
