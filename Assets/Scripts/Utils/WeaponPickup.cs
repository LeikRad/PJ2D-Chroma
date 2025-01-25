using UnityEngine;

public class WeaponPickup : MonoBehaviour
{
    public GameObject weaponPrefab;
    
    private void Start()
    {
        Invoke(nameof(CheckWeapon), 0.1f);
    }
    
    private void CheckWeapon()
    {
        PlayerWeapon playerWeapon = FindObjectOfType<PlayerWeapon>();
        playerWeapon = FindObjectOfType<PlayerWeapon>();
        if (playerWeapon != null && playerWeapon.equippedWeapon != null)
        {
            EquipWeapon(playerWeapon);
            Destroy(gameObject);
        }
    }

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
        playerWeapon.rotationPoint = newWeapon.transform.Find("RotationPoint");
        newWeapon.transform.localPosition -= playerWeapon.rotationPoint.localPosition;
        Destroy(gameObject);
        GameManager.Instance.SaveGame(); 
    }
}