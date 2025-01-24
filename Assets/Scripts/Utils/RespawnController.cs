using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RespawnController : MonoBehaviour
{
    public float attackDamage = 10f; 
    public Transform respawnPoint;
    private BossStateMachine boss;

    private void Awake()
    {
        boss = FindObjectOfType<BossStateMachine>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerHealth playerHealth = collision.gameObject.GetComponent<PlayerHealth>();

            if (playerHealth != null && CompareTag("BossLava"))
            {
                playerHealth.TakeDamage(attackDamage, transform);
                {
                    if (playerHealth.GetHealth() > 0)
                    {
                        boss.ResetPhaseFour();
                    }
                    else
                    {
                        playerHealth.Death();
                    }
                }
            }
            else
                playerHealth.TakeDamage(attackDamage, null);
            {
                if (playerHealth.GetHealth() > 0)
                {
                    collision.transform.position = respawnPoint.position;
                }
                else
                {
                    playerHealth.Death();
                }
            }
        }
    }
}
