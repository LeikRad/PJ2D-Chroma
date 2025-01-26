using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 10f;
    public int damage = 10;
    public float maxDistance = 10f;

    private Vector3 startPosition;
    
    public GameObject splatPrefab;
    public Transform splatHolder;
    private float _moveMod = 0.4f;

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
             Vector2 collisionNormal = -collision.GetContact(0).normal;
             int numberofSplats = Random.Range(1, 3);
             // each splat create first at collision and move it in direction of normal
             for (int i = 0; i < numberofSplats; i++)
             {

                 GameObject splat = Instantiate(splatPrefab,
                     collision.GetContact(0).point + _moveMod * i * collisionNormal,
                     Quaternion.identity) as GameObject;
                 splat.transform.SetParent(splatHolder, true);
                 Splat splatScript = splat.GetComponent<Splat>();
                 splatScript.Initialize(Splat.SplatLocation.Foreground);
                 Rigidbody2D rb = splat.GetComponent<Rigidbody2D>();
                 Destroy(gameObject);
             }
         }
         else
        {
            Destroy(gameObject);
        }
    }
}