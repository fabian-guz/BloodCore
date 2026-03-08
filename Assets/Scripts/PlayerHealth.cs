using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    public int health = 10;
    public int maxHealth = 10;

    private bool isDead = false;

    private PlayerMovement playerMovement;
    private GunShoot gunShoot;
    private MouseLook mouseLook;
    private CameraShake cameraShake;
    private PlayerDamageFeedback damageFeedback;

    void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();
        damageFeedback = GetComponent<PlayerDamageFeedback>();

        if (Camera.main != null)
        {
            mouseLook = Camera.main.GetComponent<MouseLook>();
            gunShoot = Camera.main.GetComponentInChildren<GunShoot>();
            cameraShake = Camera.main.GetComponent<CameraShake>();
        }

        if (UIManager.instance != null)
        {
            UIManager.instance.UpdateHealth(health);
        }

        if (damageFeedback != null)
        {
            damageFeedback.UpdateHealthVignette(health, maxHealth);
        }
    }

    public void TakeDamage(int damage)
    {
        if (isDead)
        {
            return;
        }

        health -= damage;

        if (health < 0)
        {
            health = 0;
        }

        if (UIManager.instance != null)
        {
            UIManager.instance.UpdateHealth(health);
        }

        if (cameraShake != null)
        {
            cameraShake.Shake(0.12f, 0.08f);
        }

        if (damageFeedback != null)
        {
            damageFeedback.PlayDamageSound();
            damageFeedback.UpdateHealthVignette(health, maxHealth);
        }

        if (health <= 0)
        {
            Die();
        }
    }

    public void Heal(int amount)
    {
        if (isDead)
        {
            return;
        }

        health += amount;

        if (health > maxHealth)
        {
            health = maxHealth;
        }

        if (UIManager.instance != null)
        {
            UIManager.instance.UpdateHealth(health);
        }

        if (damageFeedback != null)
        {
            damageFeedback.UpdateHealthVignette(health, maxHealth);
        }
    }

    void Die()
    {
        isDead = true;

        if (playerMovement != null)
        {
            playerMovement.enabled = false;
        }

        if (mouseLook != null)
        {
            mouseLook.enabled = false;
        }

        if (gunShoot != null)
        {
            gunShoot.enabled = false;
        }

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        if (GameManager.instance != null)
        {
            GameManager.instance.StopMusic();
            GameManager.instance.PlayGameOverSound();
        }

        if (UIManager.instance != null)
        {
            UIManager.instance.ShowGameOver();
        }
    }

    void Update()
    {
        if (isDead && Input.GetKeyDown(KeyCode.R))
        {
            Time.timeScale = 1f;
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}