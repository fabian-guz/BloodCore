using UnityEngine;
using System.Collections;
using TMPro;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float speed = 5f;
    public float gravity = -9.81f;

    [Header("Dash Settings")]
    public float dashForce = 20f;
    public float dashCooldown = 5f;
    public float dashDuration = 0.2f;
    public KeyCode dashKey = KeyCode.LeftShift; //Button for the dash

    [Header("Jump Settings")]
    public float jumpHeight = 1.5f; // Height of the jump
    public float jumpCooldown = 10f; // Cooldown time between jumps
    public KeyCode jumpKey = KeyCode.Space; //Button for the jump
    public float coyoteTime = 0.15f; // Time window to allow jump input after leaving a platform

    [Header("Skill Settings")] 
    public string dashSkillID = "Dash_01"; // Dash skill ID
    public string dashCooldown4SecSkillID = "Dash_02"; // If the player has this skill, the dash cooldown will be reduced to 4 seconds
    public string dashCooldown3SecSkillID = "Dash_03"; // If the player has this skill, the dash cooldown will be reduced to 3 seconds
    public string jumpSkillID = "Jump_01"; // Jump skill ID

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip dashSound;
    public AudioClip jumpSound;

    [Header("Effects")]
    public CameraShake cameraShake; // Reference to the CameraShake script

    private CharacterController controller;
    private Vector3 velocity;
    private float nextDashTime = 0f;
    private float nextJumpTime = 0f;
    private bool isDashing = false;

    private bool canDash;
    private bool canJump;
    private float coyoteTimeCounter = 0f;

    void Start()
    {
        controller = GetComponent<CharacterController>();

        if (ExperienceManager.instance != null && ExperienceManager.instance.IsSkillUnlocked(dashSkillID))
        {
            CheckDashSkillCooldown();
        }

        canDash = ExperienceManager.instance != null && ExperienceManager.instance.IsSkillUnlocked(dashSkillID);
        canJump = ExperienceManager.instance != null && ExperienceManager.instance.IsSkillUnlocked(jumpSkillID);
    }

    void Update()
    {
        // Dont update the DashUI before unlocked
        if (canDash)
        {
            UIManager.instance.UpdateDashUI(nextDashTime);
        }

        //Pauses the game while dashing
        if (isDashing)
        {
            return;
        }

        if (controller.isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        if (controller.isGrounded)
        {
            coyoteTimeCounter = coyoteTime; // Reset coyote time when grounded
        }
        else
        {
            coyoteTimeCounter -= Time.deltaTime; // Decrease coyote time when in the air
        }
        
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;
        move = Vector3.ClampMagnitude(move, 1f);

        controller.Move(move * speed * Time.deltaTime);

        //Dash Logic
        if (canDash && Input.GetKeyDown(dashKey) && Time.time >= nextDashTime)
        {
            StartCoroutine(Dash());
        }

        //Jump Logic
        if (canJump && Input.GetKeyDown(jumpKey) && coyoteTimeCounter > 0f && Time.time >= nextJumpTime)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            coyoteTimeCounter = 0f;
            nextJumpTime = Time.time + jumpCooldown;
            PlayJumpSound();
        }

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    void PlayDashSound()
    {
        if (audioSource != null && dashSound != null)
        {
            audioSource.PlayOneShot(dashSound);
        }
    }

    void PlayJumpSound()
    {
        if (audioSource != null && jumpSound != null)
        {
            audioSource.PlayOneShot(jumpSound);
        }
    }

    void CheckDashSkillCooldown()
    {
        if (ExperienceManager.instance.IsSkillUnlocked(dashCooldown3SecSkillID))
        {
            dashCooldown = 3f;
            return;
        }
        if (ExperienceManager.instance.IsSkillUnlocked(dashCooldown4SecSkillID))
        {
            dashCooldown = 4f;
            return;
        }
    }


    IEnumerator Dash()
    {
        isDashing = true;
        nextDashTime = Time.time + dashCooldown;
        PlayDashSound();

        if (cameraShake != null)
        {
            cameraShake.Shake(0.15f, 0.25f);
        }

        //Safes the time at the start of the Dash
        float startTime = Time.time;

        while (Time.time < startTime + dashDuration)
        {
            //Performe the dash
            controller.Move(transform.forward * dashForce * Time.deltaTime);

            //Wait till next frame
            yield return null;
        }
        isDashing = false;
    }
}