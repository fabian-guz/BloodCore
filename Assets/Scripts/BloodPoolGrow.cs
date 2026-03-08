using UnityEngine;

public class BloodPoolGrow : MonoBehaviour
{
    public float growTime = 0.4f;

    private Vector3 targetScale;
    private float timer = 0f;

    void Start()
    {
        targetScale = transform.localScale;

        transform.localScale = targetScale * 0.1f;
    }

    void Update()
    {
        if (timer < growTime)
        {
            timer += Time.deltaTime;

            float t = timer / growTime;

            transform.localScale = Vector3.Lerp(targetScale * 0.1f, targetScale, t);
        }
    }
}