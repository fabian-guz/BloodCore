using UnityEngine;
using TMPro;
using System.Collections;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI healthText;
    public TextMeshProUGUI crosshairText;
    public TextMeshProUGUI waveText;
    public TextMeshProUGUI ammoText;
    public TextMeshProUGUI pickupPopupText;

    public GameObject gameOverText;
    public GameObject hitMarkerText;
    public GameObject reloadText;

    private Coroutine pickupPopupCoroutine;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        if (crosshairText != null)
        {
            crosshairText.gameObject.SetActive(true);
        }

        if (gameOverText != null)
        {
            gameOverText.SetActive(false);
        }

        if (hitMarkerText != null)
        {
            hitMarkerText.SetActive(false);
        }

        if (reloadText != null)
        {
            reloadText.SetActive(false);
        }

        if (pickupPopupText != null)
        {
            pickupPopupText.text = "";
            pickupPopupText.gameObject.SetActive(false);
        }
    }

    public void UpdateScore(int score)
    {
        if (scoreText != null)
        {
            scoreText.text = "Score: " + score;
        }
    }

    public void UpdateHealth(int health)
    {
        if (healthText != null)
        {
            healthText.text = "Health: " + health;
        }
    }

    public void UpdateWave(int wave)
    {
        if (waveText != null)
        {
            waveText.text = "Wave: " + wave;
        }
    }

    public void UpdateAmmo(int currentAmmo, int reserveAmmo)
    {
        if (ammoText != null)
        {
            ammoText.text = "Ammo: " + currentAmmo + " / " + reserveAmmo;
        }
    }

    public void ShowReloadText()
    {
        if (reloadText != null)
        {
            reloadText.SetActive(true);
        }
    }

    public void HideReloadText()
    {
        if (reloadText != null)
        {
            reloadText.SetActive(false);
        }
    }

    public void ShowGameOver()
    {
        if (crosshairText != null)
        {
            crosshairText.gameObject.SetActive(false);
        }

        if (gameOverText != null)
        {
            gameOverText.SetActive(true);
        }

        if (reloadText != null)
        {
            reloadText.SetActive(false);
        }
    }

    public void ShowHitMarker()
    {
        StartCoroutine(ShowHitMarkerRoutine());
    }

    IEnumerator ShowHitMarkerRoutine()
    {
        if (hitMarkerText != null)
        {
            hitMarkerText.SetActive(true);
            yield return new WaitForSeconds(0.1f);
            hitMarkerText.SetActive(false);
        }
    }

    public void ShowPickupPopup(string message)
    {
        if (pickupPopupText == null)
        {
            Debug.Log("pickupPopupText ist null");
            return;
        }

        if (pickupPopupCoroutine != null)
        {
            StopCoroutine(pickupPopupCoroutine);
        }

        pickupPopupCoroutine = StartCoroutine(ShowPickupPopupRoutine(message));
    }

    IEnumerator ShowPickupPopupRoutine(string message)
    {
        pickupPopupText.text = message;
        pickupPopupText.gameObject.SetActive(true);

        yield return new WaitForSeconds(1.2f);

        pickupPopupText.gameObject.SetActive(false);
    }
}