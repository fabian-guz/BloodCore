using UnityEngine;
using System;

public class Floatingcore : MonoBehaviour
{
    [Header("Floating Settings")]
    public float bobHeight = 0.4f; //Height of the bobbing effect
    public float bobSpeed = 2f;
    public float hoverHeightAboveEnemy = 2f;

    [Header("Rotation Settings")]
    public float rotationSpeed = 90f; 

    [Header("Projectile-Movement")]
    public float launchSpeed = 12f;
    public float returnSpeed = 8f;
    public float returnThreeshold = 0.3f; //Distance at which the projectile is considered to have returned
    public float lifeTime = 4f;
    public float aimHeightOffset = 1.2f; //Height offset added to the player's position when aiming to better target the player's body instead of feet

    [Range(0f, 1f)]
    public float homingstrength = 0.15f;


    [Header("Visuals")]
    public TrailRenderer trailRenderer; //Reference to the Trail Renderer component

    //Says if the core is ready to be launched or is currently in use
    public bool IsReady => state == CoreState.Idle;

    private enum CoreState { Idle, Launched, Returning }
    private CoreState state = CoreState.Idle; //Initial state of the core

    private Transform owner; //The enemy that owns this core
    private Transform target; //The player that the core is targeting
    private Vector3 localRestPosition; //The local position where the core hovers when idle
    private Vector3 flightDirection;
    private float bobTimer = 0f;
    private float launchTimer = 0f;

    private int damage;
    private Action onReturnedCallback;
    private bool hasHit = false; //Flag to track if the projectile has hit the player to then decrease the player's health

    void Awake()
    {
        owner = transform.parent;

        localRestPosition = new Vector3(0, hoverHeightAboveEnemy, 0);
        transform.localPosition = localRestPosition;
    }

    void Update()
    {
        switch (state)
        {
            case CoreState.Idle:
                UpdateBob();
                break;
            
            case CoreState.Launched:
                UpdateLaunched();
                break;
            
            case CoreState.Returning:
                UpdateReturning();
                break;
        }

        //Always rotate the core around its own axis
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime, Space.World);
    }

    void UpdateBob()
    {
            bobTimer += Time.deltaTime * bobSpeed;
            float offsetY = Mathf.Sin(bobTimer) * bobHeight;
            transform.localPosition = localRestPosition + new Vector3(0, offsetY, 0);
    }

    void UpdateLaunched()
    {
        launchTimer += Time.deltaTime;

        if (launchTimer >= lifeTime)
        {
            SetTrailActive(false);
            state = CoreState.Returning;
            return;
        }

        if (target == null)
        {
            state = CoreState.Returning;
            return;
        }

        Vector3 toPlayer = (target.position + Vector3.up * aimHeightOffset - transform.position).normalized;
        flightDirection = Vector3.Slerp(flightDirection, toPlayer, homingstrength * Time.deltaTime * 5f).normalized;
        
        transform.position += flightDirection * launchSpeed * Time.deltaTime;

        if(!hasHit && Vector3.Distance(transform.position, target.position) < 0.7f) //If the core is close enough to the player and hasn't hit yet, consider it a hit
        {
            OnHitPlayer();
        }
    }

    void UpdateReturning()
    {
        if (owner == null)
        {
            Destroy(gameObject); //If the owner is destroyed, destroy the core as well
            return;
        }

        Vector3 worldRestPos = owner.TransformPoint(localRestPosition); //Calculate the world position of the rest position
        Vector3 direction = (worldRestPos - transform.position).normalized; //Move towards the rest position
        transform.position += direction * returnSpeed * Time.deltaTime;

        float distToRest = Vector3.Distance(transform.position, worldRestPos);
        if (distToRest < returnThreeshold)
        {
            ArriveBack();
        }
    }

    void OnHitPlayer()
    {
        hasHit = true;

        PlayerHealth health = target.GetComponent<PlayerHealth>();
        if (health != null)
        {
            health.TakeDamage(damage);
        }

        SetTrailActive(false); //Disable the trail when hitting the player
        state = CoreState.Returning; //Start returning immediately after hitting the player
    }

    void ArriveBack()
    {
        SetTrailActive(false); //Disable the trail when back at the enemy

        if (owner != null)
        {
            transform.SetParent(owner); //Reparent to the enemy
        }

        transform.localPosition = localRestPosition;

        state = CoreState.Idle;
        hasHit = false; //Reset hit flag for the next launch

        onReturnedCallback?.Invoke(); //Invoke the callback to notify that the core has returned
        onReturnedCallback = null; //Clear the callback reference
    }

    public void Launch(Transform playerTarget, int attackDamage, Action onReturned)
    {
        if (state != CoreState.Idle)
        {
            Debug.LogWarning("Floatingcore: Attempted to launch while not idle.");
            return;
        }

        target = playerTarget;
        damage = attackDamage;
        onReturnedCallback = onReturned;
        hasHit = false;
        launchTimer = 0f;

        //Flight direction to the player at the moment of launch, will then be adjusted over time in UpdateLaunched to create a homing effect
        Vector3 aimTarget = playerTarget.position + Vector3.up * aimHeightOffset; //Aim at the player's body instead of feet
        flightDirection = (aimTarget - transform.position).normalized; 

        transform.SetParent(null); //Unparent from the enemy to move freely

        SetTrailActive(true); //Enable the trail when launching
        state = CoreState.Launched;
    }

    void SetTrailActive(bool active)
    {
        if (trailRenderer != null)
        {
            trailRenderer.emitting = active;
        }
    }

    void OnEnable()
    {
        state = CoreState.Idle;
        transform.localPosition = localRestPosition;
    }
}
