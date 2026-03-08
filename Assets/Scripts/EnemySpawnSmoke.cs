using UnityEngine;

public class EnemySpawnSmoke : MonoBehaviour
{
    [SerializeField] private GameObject smokePrefab;
    [SerializeField] private Vector3 spawnOffset = new Vector3(0f, 0.2f, 0f);

    private void OnEnable()
    {
        SpawnSmoke();
    }

    private void SpawnSmoke()
    {
        if (smokePrefab == null)
        {
            return;
        }

        Vector3 spawnPosition = transform.position + spawnOffset;

        Instantiate(smokePrefab, spawnPosition, Quaternion.identity);
    }
}