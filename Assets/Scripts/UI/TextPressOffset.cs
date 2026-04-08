using UnityEngine;
using UnityEngine.EventSystems;

public class PressOffsetUI : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler
{
    [Header("Targets (optional)")]
    public RectTransform textRect;
    public RectTransform imageRect;

    [Header("Config")]
    public float offsetY = -10f;

    private Vector2 textOriginalPos;
    private Vector2 imageOriginalPos;

    void Start()
    {
        if (textRect != null)
            textOriginalPos = textRect.anchoredPosition;

        if (imageRect != null)
            imageOriginalPos = imageRect.anchoredPosition;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (textRect != null)
            textRect.anchoredPosition = textOriginalPos + new Vector2(0, offsetY);

        if (imageRect != null)
            imageRect.anchoredPosition = imageOriginalPos + new Vector2(0, offsetY);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        ResetPosition();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        ResetPosition(); 
    }

    void ResetPosition()
    {
        if (textRect != null)
            textRect.anchoredPosition = textOriginalPos;

        if (imageRect != null)
            imageRect.anchoredPosition = imageOriginalPos;
    }
}