using UnityEngine;
using System.Collections;

public class MuzzleShotLight : MonoBehaviour
{
    [SerializeField] private Light lightSource;
    [SerializeField] private float flashIntensity = 4f;
    [SerializeField] private float flashDuration = 0.04f;

    private Coroutine flashRoutine;
    private float defaultIntensity = 0f;

    private void Awake()
    {
        if (lightSource == null)
        {
            lightSource = GetComponent<Light>();
        }

        if (lightSource != null)
        {
            defaultIntensity = 0f;
            lightSource.intensity = 0f;
            lightSource.enabled = false;
        }
    }

    private void OnEnable()
    {
        if (lightSource == null)
        {
            lightSource = GetComponent<Light>();
        }

        if (lightSource != null)
        {
            lightSource.intensity = 0f;
            lightSource.enabled = false;
        }
    }

    public void Flash()
    {
        if (lightSource == null)
        {
            return;
        }

        if (flashRoutine != null)
        {
            StopCoroutine(flashRoutine);
        }

        flashRoutine = StartCoroutine(FlashRoutine());
    }

    private IEnumerator FlashRoutine()
    {
        lightSource.enabled = true;
        lightSource.intensity = flashIntensity;

        yield return new WaitForSeconds(flashDuration);

        lightSource.intensity = defaultIntensity;
        lightSource.enabled = false;
        flashRoutine = null;
    }
}