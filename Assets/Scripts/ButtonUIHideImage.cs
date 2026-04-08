using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonUIHideImage : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public GameObject targetImage; 

    public void OnPointerDown(PointerEventData eventData)
    {
        if (targetImage != null)
        {
            targetImage.SetActive(false); 
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (targetImage != null)
        {
            targetImage.SetActive(true); 
        }
    }
}