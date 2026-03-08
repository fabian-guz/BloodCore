using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    public Transform[] spawnPoints;

    public float timeBetweenWaves = 3f;
    public int currentWave = 1;
    public int enemiesPerWave = 3;

    public float delayBetweenEnemySpawns = 0.6f;
    public float minDistanceToPlayer = 6f;
    public float minDistanceToOtherEnemies = 2f;
    public float randomSpawnRadius = 2f;
    public int maxSpawnAttempts = 10;

    public AudioClip spawnSound;

    public AudioClip waveVoice;
    public AudioClip[] numberVoices;

    private int enemiesAlive = 0;
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
        enemiesAlive = enemiesPerWave;

        for (int i = 0; i < enemiesPerWave; i++)
        {
            SpawnEnemy();
            yield return new WaitForSeconds(delayBetweenEnemySpawns);
        }

        isSpawningWave = false;
    }

    void SpawnEnemy()
    {
        Vector3 spawnPosition;

        if (!TryFindValidSpawnPosition(out spawnPosition))
        {
            Debug.LogWarning("Kein guter Spawnpunkt gefunden");
            enemiesAlive--;
            return;
        }

      

        if (spawnSound != null)
        {
            AudioHelper.PlayClipAtPosition(spawnSound, spawnPosition, 0.9f);
        }

        GameObject enemy = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);

        EnemyHealth enemyHealth = enemy.GetComponent<EnemyHealth>();
        if (enemyHealth != null)
        {
            enemyHealth.spawner = this;
        }
    }

    bool TryFindValidSpawnPosition(out Vector3 validPosition)
    {
        for (int attempt = 0; attempt < maxSpawnAttempts; attempt++)
        {
            Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];

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
        enemiesPerWave += 2;

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
                yield return new WaitForSeconds(0.2f);
            }
        }

        if (ones < numberVoices.Length && numberVoices[ones] != null)
        {
            audioSource.PlayOneShot(numberVoices[ones], 0.7f);
        }
    }
}