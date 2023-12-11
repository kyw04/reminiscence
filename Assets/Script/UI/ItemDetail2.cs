using UnityEngine;
using UnityEngine.EventSystems;

public class ItemDetail2 : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{
    public GameObject Detail_1;

    private bool isPointerDown = false;
    private bool isHovering = false;

    public void OnPointerEnter(PointerEventData eventData)
    {
        isHovering = true;
        if (isPointerDown)
        {
            Detail_1.SetActive(true);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isHovering = false;
        Detail_1.SetActive(false);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        isPointerDown = true;
        if (isHovering)
        {
            Detail_1.SetActive(true);
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isPointerDown = false;
        Detail_1.SetActive(false);
    }
}