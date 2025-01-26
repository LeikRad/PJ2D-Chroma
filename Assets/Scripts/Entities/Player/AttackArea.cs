using UnityEngine;

public class AttackArea : MonoBehaviour
{
    private int damage = 3;
    private void OnTriggerEnter2D(Collider2D collider)
    {
        Debug.Log("Collided with" + collider.gameObject.name);
        if (collider.GetComponent<Health>() != null)
        {
            Health health = collider.GetComponent<Health>();
            health.TakeDamage(damage);
        }
    }

}