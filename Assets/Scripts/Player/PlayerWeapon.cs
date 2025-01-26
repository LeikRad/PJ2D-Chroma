using UnityEngine;

public class PlayerWeapon : MonoBehaviour
{
    public Transform weaponHolder;
    public GameObject equippedWeapon;
    public Transform firePoint;
    public GameObject bulletPrefab;
    public float bulletSpeed = 10f;

    public InkMeter inkMeter; // Reference to the ink meter script

    void Update()
    {
        if (equippedWeapon)
        {
            AimWeapon();
            if (Input.GetMouseButtonDown(0))
            {
                if (inkMeter != null && inkMeter.currentInk >= inkMeter.inkConsumption)
                {
                    Fire();
                    inkMeter.ConsumeInk(); // Reduce ink when firing
                }
                else
                {
                    Debug.Log("Not enough ink to shoot!");
                }
            }
        }
    }

    private void AimWeapon()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0;
        Vector2 direction = (mousePosition - weaponHolder.position).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        weaponHolder.rotation = Quaternion.Euler(0, 0, angle);
    }

    private void Fire()
    {
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        rb.linearVelocity = firePoint.right * bulletSpeed;
    }
}