using System;
using UnityEngine;

public class FallingPlatform : MonoBehaviour
{
    private Rigidbody2D rb;
    private Vector3 startPosition;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.isKinematic = true; 
        startPosition = transform.position;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log($"Collision with: {collision.gameObject.name}");
        if (collision.gameObject.CompareTag("Bullet")) 
        {
            rb.isKinematic = false; 
        }
        else if (collision.gameObject.CompareTag("Floor")) 
        {
            rb.isKinematic = true; 
        }
    }
}