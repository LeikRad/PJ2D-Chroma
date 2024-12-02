using System;
using Unity.VisualScripting.Dependencies.Sqlite;
using UnityEngine;

public class FallingPlatform : MonoBehaviour
{
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.isKinematic = true; 
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Bullet")) 
        {
            rb.isKinematic = false; 
        }
        // TODO: FIX THIS SHIT
        else if (collision.gameObject.CompareTag("Respawn"))
        {
            rb.isKinematic = true; 
            rb.constraints = RigidbodyConstraints2D.FreezeAll;
        }
    }
}