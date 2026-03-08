using UnityEngine;

public class CrosshairReaction : MonoBehaviour
{
    public float normalScale = 1f;
    public float shootScale = 1.2f;
    public float returnSpeed = 12f;

    private RectTransform rectTransform;
    private Vector3 targetScale;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        targetScale = Vector3.one * normalScale;
        rectTransform.localScale = targetScale;
    }

    void Update()
    {
        rectTransform.localScale = Vector3.Lerp(
            rectTransform.localScale,
            targetScale,
            returnSpeed * Time.deltaTime
        );

        targetScale = Vector3.Lerp(
            targetScale,
            Vector3.one * normalScale,
            returnSpeed * Time.deltaTime
        );
    }

    public void Pulse()
    {
        targetScale = Vector3.one * shootScale;
    }
}