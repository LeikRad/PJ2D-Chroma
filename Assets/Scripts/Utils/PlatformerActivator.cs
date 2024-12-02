using System;
using UnityEngine;

public class PlatformerActivator : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public Transform platforms;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            platforms.gameObject.SetActive(true);
        }
    }
}
