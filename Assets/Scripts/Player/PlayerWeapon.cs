using UnityEngine;

public class PlayerWeapon : MonoBehaviour
{
    public Transform weaponHolder; // O ponto onde a arma será presa
    public GameObject equippedWeapon; // A arma equipada
    public Transform firePoint; // O ponto de disparo da bala
    public GameObject bulletPrefab; // Prefab da bala
    public float bulletSpeed = 10f;

    void Update()
    {
        if (equippedWeapon != null)
        {
            AimWeapon();
            if (Input.GetMouseButtonDown(0)) // Botão esquerdo do mouse
            {
                Fire();
            }
        }
    }

    void AimWeapon()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0;
        Vector3 direction = mousePosition - weaponHolder.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        weaponHolder.rotation = Quaternion.Euler(0, 0, angle);
    }

    void Fire()
    {
        if (firePoint == null) return;

        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        rb.linearVelocity = firePoint.right * bulletSpeed;
    }
}