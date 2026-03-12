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

    [Header("UI Settings")]
    public TextMeshProUGUI dashStatusText;

    private CharacterController controller;
    private Vector3 velocity;
    private float nextDashTime = 0f;
    private bool isDashing = false;

    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        UpdateDashUI();
        //Pauses the game while dashing
        if (isDashing)
        {
            return;
        }
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;
        move = Vector3.ClampMagnitude(move, 1f);

        controller.Move(move * speed * Time.deltaTime);

        //Dash Logic
        if (Input.GetKeyDown(dashKey) && Time.time >= nextDashTime)
        {
            StartCoroutine(Dash());
        }

        if (controller.isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    void UpdateDashUI()
    {
        if (dashStatusText == null)
        {
            return;
        }
        if (Time.time < nextDashTime)
        {
            //Calc remaining seconds
            float remainingTime = nextDashTime - Time.time;
            dashStatusText.text = string.Format("Dash in: {0:F1}s", remainingTime);
            dashStatusText.color = Color.red;
        }
        else
        {
            dashStatusText.text = "Dash ready (SHIFT)";
            dashStatusText.color = Color.yellow;
        }
    }


    IEnumerator Dash()
    {
        isDashing = true;
        nextDashTime = Time.time + dashCooldown;

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