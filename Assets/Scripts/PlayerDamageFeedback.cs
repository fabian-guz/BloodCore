using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class PlayerDamageFeedback : MonoBehaviour
{
    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip damageSound;

    [Header("Vignette")]
    public Volume globalVolume;
    public float minVignetteIntensity = 0.18f;
    public float maxVignetteIntensity = 0.5f;
    public Color vignetteColor = new Color(0.35f, 0f, 0f, 1f);

    private Vignette vignette;

    void Start()
    {
        if (globalVolume != null && globalVolume.profile != null)
        {
            globalVolume.profile.TryGet(out vignette);

            if (vignette != null)
            {
                vignette.color.overrideState = true;
                vignette.color.value = vignetteColor;

                vignette.intensity.overrideState = true;
                vignette.intensity.value = minVignetteIntensity;
            }
        }
    }

    public void PlayDamageSound()
    {
        if (audioSource != null && damageSound != null)
        {
            audioSource.PlayOneShot(damageSound);
        }
    }

    public void UpdateHealthVignette(int currentHealth, int maxHealth)
    {
        if (vignette == null)
        {
            return;
        }

        float healthPercent = (float)currentHealth / maxHealth;
        float damagePercent = 1f - healthPercent;

        float targetIntensity = Mathf.Lerp(
            minVignetteIntensity,
            maxVignetteIntensity,
            damagePercent
        );

        vignette.intensity.value = targetIntensity;
    }
}