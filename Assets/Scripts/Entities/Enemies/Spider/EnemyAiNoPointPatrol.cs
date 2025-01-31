﻿using System;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyAiNoPointPatrol : MonoBehaviour
{
    public float patrolDistance = 2f; 
    public float speed = 2f;         
    public Transform player;        
    public float detectionRange = 10f; 
    private Vector3 spawnPosition;  
    private bool movingRight = true; 
    private bool isChasingPlayer = false; 
    private bool isPlayerAlive = true;

    void Start()
    {
        spawnPosition = transform.position; 
        player = GameObject.FindGameObjectWithTag("Player").transform;  
    }

    void Update()
    {
        if (!isPlayerAlive || IsPlayerInRange())
        {
            ChasePlayer();
        }
        else
        {
            Patrol();
        }
    }

    void Patrol()
    {
       
        float leftBoundary = spawnPosition.x - patrolDistance;
        float rightBoundary = spawnPosition.x + patrolDistance;

        
        if (movingRight)
        {
            transform.Translate(Vector3.right * speed * Time.deltaTime);
            if (transform.position.x >= rightBoundary)
            {
                movingRight = false;
            }
        }
        else
        {
            transform.Translate(Vector3.left * speed * Time.deltaTime);
            if (transform.position.x <= leftBoundary)
            {
                movingRight = true;
            }
        }
    }

    void ChasePlayer()
    {
        
        Vector3 direction = (player.position - transform.position).normalized;
        transform.Translate(direction * speed * Time.deltaTime);
    }

    bool IsPlayerInRange()
    {
       
        float distanceToPlayer = Vector3.Distance(player.position, transform.position);
        bool inRange = distanceToPlayer <= detectionRange;
        return inRange;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            PlayerHealth player = other.gameObject.GetComponent<PlayerHealth>();
            if (player != null)
            {
                player.TakeDamage(10, transform);
                if (player.GetHealth() <= 0)
                {
                    isPlayerAlive = false;
                    player.Death();
                }
            }
        }
    }
}
