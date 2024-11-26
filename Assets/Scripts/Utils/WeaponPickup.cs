using UnityEngine;

public class WeaponPickup : MonoBehaviour
{
    public GameObject weaponPrefab;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerWeapon playerWeapon = other.GetComponent<PlayerWeapon>();
            if (playerWeapon != null)
            {
                EquipWeapon(playerWeapon);
            }
        }
    }

    private void EquipWeapon(PlayerWeapon playerWeapon)
    {
        if (playerWeapon.equippedWeapon != null)
        {
            Destroy(playerWeapon.equippedWeapon);
        }
        GameObject newWeapon = Instantiate(weaponPrefab, playerWeapon.weaponHolder);
        newWeapon.transform.localPosition = Vector3.zero;
        newWeapon.transform.localRotation = Quaternion.identity;
        playerWeapon.equippedWeapon = newWeapon;
        playerWeapon.firePoint = newWeapon.transform.Find("FirePoint");
        Destroy(gameObject);
    }
}