using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 10f;
    public int damage = 10;
    public float maxDistance = 10f;

    private Vector3 startPosition;
    
    public GameObject splatPrefab;
    public Transform splatHolder;

    private void Start()
    {
        startPosition = transform.position;
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        rb.linearVelocity = transform.right * speed; // Certifique-se de que o projétil está se movendo
        splatHolder = GameObject.Find("SplatHolder").transform;
    }

    private void Update()
    {
        if (Vector3.Distance(startPosition, transform.position) >= maxDistance)
        {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Boss"))
        {
            BossStateMachine boss = collision.gameObject.GetComponent<BossStateMachine>();
            if (boss != null)
            {
                boss.TakeDamage(damage);
            }
            Destroy(gameObject);
        }
        else if (collision.gameObject.CompareTag("Enemy"))
        {
            collision.gameObject.GetComponent<Health>()?.TakeDamage(damage);
            Destroy(gameObject);
        }
        else if (collision.gameObject.layer == LayerMask.NameToLayer("Floors"))
        {
            GameObject splat = Instantiate(splatPrefab, transform.position, Quaternion.identity);
            splat.transform.SetParent(splatHolder);
            Splat splatScript = splat.GetComponent<Splat>();
            splatScript.Initialize(Splat.SplatLocation.Foreground);
            Destroy(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}