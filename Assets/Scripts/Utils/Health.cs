using UnityEngine;
using System.Collections;


public class Health : MonoBehaviour
{
    public float maxHealth = 100f;
    public float currentHealth;

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
