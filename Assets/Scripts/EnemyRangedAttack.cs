using UnityEngine;

public class EnemyRangedAttack : MonoBehaviour
{
    [Header("Ranged Attack Settings")]
    public int attackDamage = 1;
    public float attackRange = 10f;
    public float minAttackRange = 3f;
    public float attackCooldown = 3f;

    [Header("Core-Reference")]
    public Floatingcore floatingCore; //Reference to the Floatingcore component

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

        if (floatingCore == null)
        {
            floatingCore = GetComponentInChildren<Floatingcore>();
        }

        if (player == null)
        {
            Debug.LogWarning("EnemyRangedAttack: No Player with tag 'Player' found.");
        }

        if (floatingCore == null)
        {
            Debug.LogWarning("EnemyRangedAttack: No Floatingcore component found.");
        }
    }

    void Update()
    {
        if (player == null || playerHealth == null || floatingCore == null)
        {
            return;
        }

        //Only attack if the core is ready to be launched
        if (!floatingCore.IsReady)
        {
            return;
        }

        float distance = Vector3.Distance(transform.position, player.position);

        bool inAttackRange = distance <= attackRange && distance >= minAttackRange; //Player has to be within the attack range but not too close

        if (inAttackRange && Time.time >= nextAttackTime)
        {
            floatingCore.Launch(player, attackDamage, OnCoreReturned);
            nextAttackTime = Time.time + attackCooldown;
        }
    }

    void OnCoreReturned()
    {
        return;
        //This callback is called when the core has returned to the enemy after being launched
        //You can use this to reset any states or trigger any effects if needed
    }
}
