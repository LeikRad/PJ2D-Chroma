using UnityEngine;

public class Health : MonoBehaviour
{
    public float maxHealth = 100f; 
    private float currentHealth;

    public void Start()
    {
        currentHealth = maxHealth; 
    }

    public virtual void TakeDamage(float amount)
    {
        currentHealth -= amount; 

        if (currentHealth <= 0)
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
