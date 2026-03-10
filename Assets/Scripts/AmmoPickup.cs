using UnityEngine;
using System.Collections;

public class AmmoPickup : MonoBehaviour
{
    public int ammoAmount = 10;
    public AudioClip pickupSound;

    [HideInInspector]
    public PickupSpawner spawner;

    void Start()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
        {
            return;
        }

        GunShoot gun = other.GetComponentInChildren<GunShoot>();

        if (gun == null)
        {
            return;
        }

        if (gun.reserveAmmo >= gun.maxReserveAmmo)
        {
            return;
        }

        gun.AddAmmo(ammoAmount);

        if (UIManager.instance != null)
        {
            UIManager.instance.ShowPickupPopup("+" + ammoAmount + " Ammo");
        }

        if (pickupSound != null)
        {
            AudioHelper.PlayClipAtPosition(pickupSound, transform.position, 0.7f);
        }

        if (spawner != null)
        {
            spawner.RequestRespawn();
        }

        Destroy(gameObject);
    }
}