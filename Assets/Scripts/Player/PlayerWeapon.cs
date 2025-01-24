using UnityEngine;

public class PlayerWeapon : MonoBehaviour
{
    public Transform weaponHolder;
    public GameObject equippedWeapon;
    public Transform firePoint;
    public Transform rotationPoint;
    public GameObject bulletPrefab;
    public float bulletSpeed = 10f;

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
        weaponHolder.rotation = Quaternion.Euler(0, 0, angle);
    }

    private void Fire()
    {
        if (firePoint == null)
        {
            FindFirePoint(); 
        }

        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        rb.linearVelocity = firePoint.right * bulletSpeed;
    }

    public void EquipDefaultWeapon()
    {
        if (equippedWeapon != null)
        {
            Destroy(equippedWeapon);
        }
        
        GameObject weaponPrefab = Resources.Load<GameObject>("Prefabs/Weapon/DefaultWeapon"); 
        equippedWeapon = Instantiate(weaponPrefab, weaponHolder);
        equippedWeapon.transform.localPosition = Vector3.zero;
        equippedWeapon.transform.localRotation = Quaternion.identity;
        FindFirePoint(); 
    }

    private void FindFirePoint()
    {
        if (equippedWeapon != null)
        {
            firePoint = equippedWeapon.transform.Find("FirePoint");
            rotationPoint = equippedWeapon.transform.Find("RotationPoint");
        }
    }
}
