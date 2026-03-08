using UnityEngine;
using System.Collections;

public class AmmoPickup : MonoBehaviour
{
    public int ammoAmount = 10;
    public float respawnTime = 10f;
    public AudioClip pickupSound;

    private MeshRenderer meshRenderer;
    private Collider pickupCollider;
    private Rigidbody rb;
    private bool isAvailable = true;

    void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        pickupCollider = GetComponent<Collider>();
        rb = GetComponent<Rigidbody>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!isAvailable)
        {
            return;
        }

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

        StartCoroutine(RespawnRoutine());
    }

    IEnumerator RespawnRoutine()
    {
        isAvailable = false;

        meshRenderer.enabled = false;
        pickupCollider.enabled = false;

        if (rb != null)
        {
            rb.detectCollisions = false;
        }

        yield return new WaitForSeconds(respawnTime);

        meshRenderer.enabled = true;
        pickupCollider.enabled = true;

        if (rb != null)
        {
            rb.detectCollisions = true;
        }

        isAvailable = true;
    }
}