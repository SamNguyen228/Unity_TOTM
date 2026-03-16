using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonUIHideImage : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public GameObject targetImage; // Image kéo vào Inspector

    public void OnPointerDown(PointerEventData eventData)
    {
        if (targetImage != null)
        {
            targetImage.SetActive(false); // Ẩn khi giữ
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (targetImage != null)
        {
            targetImage.SetActive(true); // Hiện lại khi thả tay
        }
    }
}