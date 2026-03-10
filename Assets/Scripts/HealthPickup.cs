using UnityEngine;
using System.Collections;

public class HealthPickup : MonoBehaviour
{
    public int healAmount = 3;
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

        PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();

        if (playerHealth == null)
        {
            return;
        }

        if (playerHealth.health >= playerHealth.maxHealth)
        {
            return;
        }

        playerHealth.Heal(healAmount);

        if (UIManager.instance != null)
        {
            UIManager.instance.ShowPickupPopup("+" + healAmount + " Health");
        }

        if (pickupSound != null)
        {
            AudioHelper.PlayClipAtPosition(pickupSound, transform.position, 0.7f);
        }

        if (spawner  != null)
        {
            spawner.RequestRespawn();
        }

        Destroy(gameObject);
    }
}