using UnityEngine;

public class MuzzleShotLight : MonoBehaviour
{
    public Light shotLight;
    public float flashIntensity = 4f;
    public float flashDuration = 0.03f;

    private float timer = 0f;

    void Awake()
    {
        if (shotLight == null)
        {
            shotLight = GetComponent<Light>();
        }

        if (shotLight != null)
        {
            shotLight.enabled = true;
            shotLight.intensity = 0f;
        }
    }

    void Update()
    {
        if (shotLight == null)
        {
            return;
        }

        if (timer > 0f)
        {
            timer -= Time.deltaTime;
            shotLight.intensity = flashIntensity;
        }
        else
        {
            shotLight.intensity = 0f;
        }
    }

    public void Flash()
    {
        if (shotLight == null)
        {
            return;
        }

        timer = flashDuration;
    }
}