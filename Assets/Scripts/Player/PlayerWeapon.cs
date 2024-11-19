using UnityEngine;

public class PlayerWeapon : MonoBehaviour
{
    public Transform weaponHolder;
    public GameObject equippedWeapon;
    public Transform firePoint;
    public GameObject bulletPrefab;
    public float bulletSpeed = 10f;
    private bool canShoot = true;

    void Update()
    {
        if (equippedWeapon)
        {
            AimWeapon();
            if (Input.GetMouseButtonDown(0))
            {
                Fire();
            }
        }
    }

    private void AimWeapon()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0;
        Vector2 direction = (mousePosition - weaponHolder.position).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        bool facingRight = transform.localScale.x > 0;
        bool mouseOnCorrectSide = (facingRight && mousePosition.x >= transform.position.x) || (!facingRight && mousePosition.x < transform.position.x);
        canShoot = mouseOnCorrectSide;

        if (!mouseOnCorrectSide)
        {
            return;
        }
        if (facingRight)
        {
            angle = Mathf.Clamp(angle, -90f, 90f);
        }
        else
        {
            if (angle > 0) angle = Mathf.Clamp(angle, 90f, 180f);
            else angle = Mathf.Clamp(angle, -180f, -90f);
        }
        weaponHolder.rotation = Quaternion.Euler(0, 0, angle);
    }

    private void Fire()
    {
        if (!canShoot)
        {
            return;
        }

        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        rb.linearVelocity = firePoint.right * bulletSpeed;
    }
}