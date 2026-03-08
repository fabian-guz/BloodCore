using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    public int attackDamage = 1;
    public float attackRange = 2f;
    public float attackRate = 1f;

    private Transform player;
    private PlayerHealth playerHealth;
    private float nextAttackTime = 0f;

    void Start()
    {
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");

        if (playerObject != null)
        {
            player = playerObject.transform;
            playerHealth = playerObject.GetComponent<PlayerHealth>();
        }

        if (player == null)
        {
            Debug.LogWarning("EnemyAttack: Player mit Tag 'Player' nicht gefunden.");
        }

        if (playerHealth == null)
        {
            Debug.LogWarning("EnemyAttack: PlayerHealth auf Player nicht gefunden.");
        }
    }

    void Update()
    {
        if (player == null || playerHealth == null)
        {
            return;
        }

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer <= attackRange && Time.time >= nextAttackTime)
        {
            playerHealth.TakeDamage(attackDamage);
            nextAttackTime = Time.time + attackRate;
        }
    }
}