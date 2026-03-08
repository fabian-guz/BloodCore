using UnityEngine;

public class LightFlicker : MonoBehaviour
{
    [SerializeField] private Light targetLight;
    [SerializeField] private float minIntensity = 1.2f;
    [SerializeField] private float maxIntensity = 2.0f;
    [SerializeField] private float flickerSpeed = 0.08f;

    private float timer;

    private void Reset()
    {
        targetLight = GetComponent<Light>();
    }

    private void Start()
    {
        if (targetLight == null)
        {
            targetLight = GetComponent<Light>();
        }

        SetNextTimer();
    }

    private void Update()
    {
        if (targetLight == null)
        {
            return;
        }

        timer -= Time.deltaTime;

        if (timer <= 0f)
        {
            targetLight.intensity = Random.Range(minIntensity, maxIntensity);
            SetNextTimer();
        }
    }

    private void SetNextTimer()
    {
        timer = Random.Range(flickerSpeed * 0.5f, flickerSpeed * 1.5f);
    }
}