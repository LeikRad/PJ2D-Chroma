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

    void EquipWeapon(PlayerWeapon playerWeapon)
    {
        if (playerWeapon.equippedWeapon != null)
        {
            Destroy(playerWeapon.equippedWeapon); // Remove a arma antiga
        }

        GameObject newWeapon = Instantiate(weaponPrefab, playerWeapon.weaponHolder);
        playerWeapon.equippedWeapon = newWeapon;
        playerWeapon.firePoint = newWeapon.transform.Find("FirePoint"); // Configura o FirePoint
        Destroy(gameObject); // Remove o pickup do cenário
    }
}