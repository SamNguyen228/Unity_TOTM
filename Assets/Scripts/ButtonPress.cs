using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonPress : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    Vector3 startPos;

    void Start()
    {
        startPos = transform.localPosition;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        transform.localPosition = startPos + Vector3.down * 4;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        transform.localPosition = startPos;
    }
}