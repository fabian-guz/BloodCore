using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public int maxHealth = 3;
    private int currentHealth;
    public EnemySpawner spawner;
    public AudioClip deathSound;

    public EnemyHitFlash enemyHitFlash;

    public int expReward = 5;

    private GunEffectsController gunEffectsController;
    private bool isDead;

    void Start()
    {
        currentHealth = maxHealth;
        gunEffectsController = FindObjectOfType<GunEffectsController>();
    }

    public void TakeDamage(int damage)
    {
        if (isDead)
        {
            return;
        }

        currentHealth -= damage;

        if (enemyHitFlash != null)
        {
            enemyHitFlash.Flash();
        }

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        if (isDead)
        {
            return;
        }
        isDead = true;

        if (ExperienceManager.instance != null)
        {
            ExperienceManager.instance.AddExperience(expReward);
        }

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