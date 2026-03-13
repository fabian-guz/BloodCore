using UnityEngine;
using System.Collections;

public class HealthPickup : MonoBehaviour
{
    [Header("Health Range")]
    public int minHealAmount = 1;
    public int maxHealAmount = 3;

    [Header("Audio")]
    public AudioClip pickupSound;

    [HideInInspector]
    public PickupSpawner spawner;

    private int healAmount;

    void Start()
    {
        healAmount = Random.Range(minHealAmount, maxHealAmount + 1);
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

        if (spawner != null)
        {
            spawner.RequestRespawn();
        }

        Destroy(gameObject);
    }
}