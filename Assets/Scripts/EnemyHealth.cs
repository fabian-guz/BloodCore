using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public int health = 3;
    public EnemySpawner spawner;
    public AudioClip deathSound;

    public EnemyHitFlash enemyHitFlash;

    private GunEffectsController gunEffectsController;

    void Start()
    {
        gunEffectsController = FindObjectOfType<GunEffectsController>();
    }

    public void TakeDamage(int damage)
    {
        health -= damage;

        if (enemyHitFlash != null)
        {
            enemyHitFlash.Flash();
        }

        if (health <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        if (gunEffectsController != null)
        {
            gunEffectsController.SpawnGroundBloodPuddle(transform.position);
        }

        if (GameManager.instance != null)
        {
            GameManager.instance.AddScore(1);
        }

        if (spawner != null)
        {
            spawner.OnEnemyKilled();
        }
        gunEffectsController.SpawnGroundBloodPuddle(transform.position);
        AudioHelper.PlayClipAtPosition(deathSound, transform.position, 0.8f);

        Destroy(gameObject);
    }
}