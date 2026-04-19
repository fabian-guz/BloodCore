using UnityEngine;
using UnityEngine.AI;
using System.Collections;

[System.Serializable]
public class EnemyType
{
    public string name;
    public GameObject prefab;  
    public int health;
    public float scale = 1f;    // Default scale is 1
    [Range(0, 100)] public int spawnChance; // Chance in percantage to spawn this enemy type
}

[System.Serializable]
public class BossEnemyType
{
    public string name;
    public GameObject prefab;  
    public int health;
    public float scale = 1f;    // Default scale is 1
}

[System.Serializable]
public class RangedEnemyType
{
    public string name;
    public GameObject prefab;
    public int health;
    public float scale = 1f;    // Default scale is 1
    public int startWave = 8;
    public int countPerWave = 0;
}

public class EnemySpawner : MonoBehaviour
{
    public EnemyType[] enemyTypes;  // Array of different enemy types to spawn
    public BossEnemyType bossEnemyType; // Boss enemy type to spawn on certain waves
    public RangedEnemyType rangedEnemyType; // Ranged enemy type to start spawning from a certain wave
    public Transform[] spawnPoints;

    public Transform[] bossSpawnPoints;

    public float timeBetweenWaves = 3f;
    public int currentWave = 1;
    public int enemiesPerWave = 3;
    public int smallEnemysPerBossWave = 2; // Number of small enemies to spawn alongside the boss on boss waves

    public float delayBetweenEnemySpawns = 0.6f;
    public float minDistanceToPlayer = 6f;
    public float minDistanceToOtherEnemies = 2f;
    public float randomSpawnRadius = 2f;
    public int maxSpawnAttempts = 10;

    public AudioClip spawnSound;

    public AudioClip waveVoice;
    public AudioClip[] numberVoices;

    private int enemiesAlive = 0;
    private int bossEnemiesAlive = 0;
    private bool isSpawningWave = false;
    private AudioSource audioSource;
    private Transform player;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        StartWave();
    }

    void Update()
    {
        if (isSpawningWave)
        {
            return;
        }

        if (enemiesAlive <= 0)
        {
            StartCoroutine(StartNextWave());
        }
    }

    void StartWave()
    {
        isSpawningWave = true;

        if (UIManager.instance != null)
        {
            UIManager.instance.UpdateWave(currentWave);
        }

        StartCoroutine(PlayWaveAnnouncement());
        StartCoroutine(SpawnWave());
    }

    IEnumerator SpawnWave()
    {
        if (currentWave % 5 == 0 && bossEnemyType != null)  // Every 5th wave, spawn a boss enemy instead of regular enemies
        {
            bossEnemiesAlive = currentWave / 5; // Set enemies alive to the number of bosses for this wave (1 boss for wave 5, 3 bosses for wave 15, etc.)
            if (currentWave == 10)
            {
                bossEnemyType.health = bossEnemyType.health * 2;
                bossEnemiesAlive = 1; // Only spawn 1 boss on wave 10, but make it stronger
            }

            enemiesAlive = bossEnemiesAlive;

            for (int i = 0; i < bossEnemiesAlive; i++)
            {
                SpawnBossEnemy();
                yield return new WaitForSeconds(delayBetweenEnemySpawns);
            }

            if(currentWave >= 15)
            {
                enemiesAlive += smallEnemysPerBossWave; // Add the small enemies to the total count of enemies alive for this wave
                for(int i = 0; i < smallEnemysPerBossWave; i++)
                {
                    SpawnEnemy(true);
                    yield return new WaitForSeconds(delayBetweenEnemySpawns);
                }
                smallEnemysPerBossWave += 2;
            }
        }
        else // Spawn regular enemies for the wave
        {
            enemiesAlive = enemiesPerWave;

            for (int i = 0; i < enemiesPerWave; i++)
            {
                SpawnEnemy(false);
                yield return new WaitForSeconds(delayBetweenEnemySpawns);
            }

            if (rangedEnemyType != null && rangedEnemyType.prefab != null && currentWave >= rangedEnemyType.startWave)
            {
                int rangedCount = rangedEnemyType.countPerWave;
                enemiesAlive += rangedCount; // Add the ranged enemies to the total count of enemies alive

                for (int i = 0; i < rangedCount; i++)
                {
                    SpawnRangedEnemy();
                    yield return new WaitForSeconds(delayBetweenEnemySpawns);
                }
            }
        }

        isSpawningWave = false; 
    }

    void SpawnEnemy(bool isBossWave)
    {
        Vector3 spawnPosition;
 
        if (!TryFindValidSpawnPosition(out spawnPosition, false))
        {
            Debug.LogWarning("No valid spawn position found for enemy. Skipping spawn.");
            enemiesAlive--;
            return;
        }
 
        EnemyType selectedType = isBossWave
            ? enemyTypes[0]
            : enemyTypes[Random.Range(0, enemyTypes.Length)];
 
        SpawnEnemyAt(selectedType.prefab, spawnPosition, selectedType.scale, selectedType.health);
    }

    void SpawnRangedEnemy()
    {
        Vector3 spawnPosition;
 
        if (!TryFindValidSpawnPosition(out spawnPosition, false))
        {
            Debug.LogWarning("No valid spawn position found for ranged enemy. Skipping spawn.");
            enemiesAlive--;
            return;
        }
 
        SpawnEnemyAt(rangedEnemyType.prefab, spawnPosition, rangedEnemyType.scale, rangedEnemyType.health);
    }

    void SpawnBossEnemy()
    {
        Vector3 spawnPosition;

        if (!TryFindValidSpawnPosition(out spawnPosition, true))
        {
            Debug.LogWarning("No valid spawn position found for boss enemy. Skipping spawn.");
            return;
        }

        GameObject bossEnemy = Instantiate(bossEnemyType.prefab, spawnPosition, Quaternion.identity);
        
        NavMeshAgent agent = bossEnemy.GetComponent<NavMeshAgent>();
        if (agent != null)
        {
            agent.enabled = false; // Disable NavMeshAgent to prevent movement issues during spawn
        }

        if (bossEnemyType.scale != 1f)
        {
            #if UNITY_EDITOR
            Debug.Log($"Spawning {bossEnemyType.name} with scale {bossEnemyType.scale}");
            #endif
        }
        bossEnemy.transform.localScale = Vector3.one * bossEnemyType.scale; // Apply scale from BossEnemyType
        float heightOffset = 0.05f * bossEnemyType.scale; // Adjust height offset based on scale
        agent.Warp(spawnPosition + Vector3.up * heightOffset); // Warp the agent to the spawn position with height offset

        EnemyHealth enemyHealth = bossEnemy.GetComponent<EnemyHealth>();
        if (enemyHealth != null)
        {
            enemyHealth.maxHealth = bossEnemyType.health;  // Set health from BossEnemyType
            enemyHealth.spawner = this;
        }

        if (agent != null)
        {
            agent.enabled = true; // Re-enable NavMeshAgent after setting up the boss enemy
        }

        if (spawnSound != null)
        {
            AudioHelper.PlayClipAtPosition(spawnSound, spawnPosition, 1.0f);
        }
    }

    void SpawnEnemyAt(GameObject prefab, Vector3 spawnPosition, float scale, int health)
    {
        GameObject enemy = Instantiate(prefab, spawnPosition, Quaternion.identity);
        NavMeshAgent agent = enemy.GetComponent<NavMeshAgent>();
 
        if (agent != null) agent.enabled = false;
 
        enemy.transform.localScale = Vector3.one * scale;
        float heightOffset = 0.1f * scale;
 
        if (agent != null)
        {
            agent.Warp(spawnPosition + Vector3.up * heightOffset);
            agent.enabled = true;
        }
 
        EnemyHealth enemyHealth = enemy.GetComponent<EnemyHealth>();
        if (enemyHealth != null)
        {
            enemyHealth.maxHealth = health;
            enemyHealth.spawner = this;
        }
 
        if (spawnSound != null){
            AudioHelper.PlayClipAtPosition(spawnSound, spawnPosition, 0.9f);
        }

        #if UNITY_EDITOR
        Debug.Log($"Spawned {prefab.name} with scale {scale}");
        #endif
    }

    bool TryFindValidSpawnPosition(out Vector3 validPosition, bool isBoss)
    {
        for (int attempt = 0; attempt < maxSpawnAttempts; attempt++)
        {
            Transform spawnPoint;
            if (!isBoss)
            {
                spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
            }
            else
            {
                spawnPoint = bossSpawnPoints[Random.Range(0, bossSpawnPoints.Length)];
            }
            
            Vector3 randomOffset = new Vector3(
                Random.Range(-randomSpawnRadius, randomSpawnRadius),
                0f,
                Random.Range(-randomSpawnRadius, randomSpawnRadius)
            );

            Vector3 candidatePosition = spawnPoint.position + randomOffset;

            NavMeshHit navHit;
            if (!NavMesh.SamplePosition(candidatePosition, out navHit, 3f, NavMesh.AllAreas))
            {
                continue;
            }

            Vector3 finalPosition = navHit.position + Vector3.up * 1f;

            if (player != null)
            {
                float playerDistance = Vector3.Distance(finalPosition, player.position);
                if (playerDistance < minDistanceToPlayer)
                {
                    continue;
                }
            }

            GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
            bool tooCloseToEnemy = false;

            foreach (GameObject enemy in enemies)
            {
                float enemyDistance = Vector3.Distance(finalPosition, enemy.transform.position);
                if (enemyDistance < minDistanceToOtherEnemies)
                {
                    tooCloseToEnemy = true;
                    break;
                }
            }

            if (tooCloseToEnemy)
            {
                continue;
            }

            validPosition = finalPosition;
            return true;
        }

        validPosition = Vector3.zero;
        return false;
    }

    public void OnEnemyKilled()
    {
        enemiesAlive--;
    }

    IEnumerator StartNextWave()
    {
        isSpawningWave = true;

        yield return new WaitForSeconds(timeBetweenWaves);

        currentWave++;

        if (currentWave >= 8 && currentWave % 4 == 0)
        {
            //Every 4 waves, increase the number of enemies by 2 but one of them is a ranged enemy starting from wave 8
            enemiesPerWave += 1;
            rangedEnemyType.countPerWave += 1;
        }
        else
        {
            enemiesPerWave += 2;
        }
        
        StartWave();
    }

    IEnumerator PlayWaveAnnouncement()
    {
        if (audioSource == null)
        {
            yield break;
        }

        if (waveVoice != null)
        {
            audioSource.PlayOneShot(waveVoice, 1.0f);
        }

        yield return new WaitForSeconds(0.6f);

        int tens = currentWave / 10;
        int ones = currentWave % 10;

        if (tens > 0)
        {
            if (tens < numberVoices.Length && numberVoices[tens] != null)
            {
                audioSource.PlayOneShot(numberVoices[tens], 0.7f);
                yield return new WaitForSeconds(0.65f);
            }
        }

        if (ones < numberVoices.Length && numberVoices[ones] != null)
        {
            audioSource.PlayOneShot(numberVoices[ones], 0.7f);
        }
    }
}