using UnityEngine;
using System.Collections;

public class EnemySpawnEffect : MonoBehaviour
{
    public float spawnProtectionTime = 1f;

    private Renderer enemyRenderer;
    private EnemyAttack enemyAttack;

    void Start()
    {
        enemyRenderer = GetComponent<Renderer>();
        enemyAttack = GetComponent<EnemyAttack>();

        if (enemyAttack != null)
        {
            enemyAttack.enabled = false;
        }

        StartCoroutine(SpawnRoutine());
    }

    IEnumerator SpawnRoutine()
    {
        if (enemyRenderer != null)
        {
            Color c = enemyRenderer.material.color;
            c.a = 0.4f;
            enemyRenderer.material.color = c;
        }

        yield return new WaitForSeconds(spawnProtectionTime);

        if (enemyRenderer != null)
        {
            Color c = enemyRenderer.material.color;
            c.a = 1f;
            enemyRenderer.material.color = c;
        }

        if (enemyAttack != null)
        {
            enemyAttack.enabled = true;
        }
    }
}