using UnityEngine;
using UnityEngine.EventSystems;

public class MenuButtonHoverScale : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private float hoverScaleMultiplier = 1.08f;
    [SerializeField] private float pressedScaleMultiplier = 0.96f;
    [SerializeField] private float scaleSpeed = 10f;

    private Vector3 baseScale;
    private Vector3 targetScale;
    private bool isHovered;

    private void Awake()
    {
        baseScale = transform.localScale;
        targetScale = baseScale;
    }

    private void Update()
    {
        transform.localScale = Vector3.Lerp(transform.localScale, targetScale, Time.unscaledDeltaTime * scaleSpeed);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        isHovered = true;
        targetScale = baseScale * hoverScaleMultiplier;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isHovered = false;
        targetScale = baseScale;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        targetScale = baseScale * pressedScaleMultiplier;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        targetScale = isHovered ? baseScale * hoverScaleMultiplier : baseScale;
    }
}