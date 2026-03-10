using UnityEngine;
using System.Collections;

public class PickupSpawner : MonoBehaviour
{
    public GameObject pickupPrefab; //  Health- or Ammo-Prefab
    public Transform[] spawnPoints; //  List of empty GameObjects as Positions
    public float respawnDelay = 10f;

    void Start()
    {
        SpawnPickup();
    }

    public void SpawnPickup()
    {
        if (spawnPoints.Length == 0) return;

        // Random point
        int randomIndex = Random.Range(0, spawnPoints.Length);
        Transform selectedPoint = spawnPoints[randomIndex];

        // Pickup instantiate
        GameObject newPickup = Instantiate(pickupPrefab, selectedPoint.position, selectedPoint.rotation);

        //  Pickup to Spawner
        if (newPickup.TryGetComponent(out HealthPickup hp)) hp.spawner = this;
        if (newPickup.TryGetComponent(out AmmoPickup ap)) ap.spawner = this;
    }

    public void RequestRespawn()
    {
        //10-second-timer
        Invoke(nameof(SpawnPickup), respawnDelay);
    }
}