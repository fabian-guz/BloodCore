using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public float defaultDuration = 0.12f;
    public float defaultMagnitude = 0.08f;
    public float returnSpeed = 18f;

    private Vector3 originalLocalPosition;
    private float shakeTimer = 0f;
    private float currentMagnitude = 0f;

    void Start()
    {
        originalLocalPosition = transform.localPosition;
    }

    void LateUpdate()
    {
        if (shakeTimer > 0f)
        {
            Vector3 randomOffset = Random.insideUnitSphere * currentMagnitude;
            randomOffset.z = 0f;

            transform.localPosition = originalLocalPosition + randomOffset;

            shakeTimer -= Time.deltaTime;
        }
        else
        {
            shakeTimer = 0f;
            transform.localPosition = Vector3.Lerp(
                transform.localPosition,
                originalLocalPosition,
                returnSpeed * Time.deltaTime
            );
        }
    }

    public void Shake()
    {
        shakeTimer = defaultDuration;
        currentMagnitude = defaultMagnitude;
    }

    public void Shake(float duration, float magnitude)
    {
        shakeTimer = duration;
        currentMagnitude = magnitude;
    }
}