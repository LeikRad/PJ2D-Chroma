using UnityEngine;

public class PlatformerActivator : MonoBehaviour
{
    public static PlatformerActivator Instance { get; private set; }
    public Transform platforms;
    private PlayerWeapon playerWeapon;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        Invoke(nameof(CheckWeaponStatus), 0.1f);
    }

    private void CheckWeaponStatus()
    {
        playerWeapon = FindObjectOfType<PlayerWeapon>();
        if (playerWeapon != null && playerWeapon.equippedWeapon != null)
        {
            ActivatePlatforms();
        }
    }

    public void ActivatePlatforms()
    {
        platforms.gameObject.SetActive(true);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            platforms.gameObject.SetActive(true);
        }
    }
}