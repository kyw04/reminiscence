using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Itemdetail : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject ItemdetailObject;
    public Canvas canvas;
    public float offset = 2f;
    public float distanceFromMouse = 20f; // 마우스와의 거리

    private bool isHovering = false;
    private RectTransform canvasRectTransform;

    private void Start()
    {
        canvasRectTransform = canvas.GetComponent<RectTransform>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        isHovering = true;
        ItemdetailObject.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isHovering = false;
        ItemdetailObject.SetActive(false);
    }

    private void Update()
    {
        if (isHovering)
        {
            Vector2 mousePosition = Input.mousePosition;
            Vector2 localPoint;

            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                canvasRectTransform,
                mousePosition,
                null,
                out localPoint
            );

            ItemdetailObject.transform.position = canvasRectTransform.TransformPoint(localPoint) + Vector3.forward * distanceFromMouse;
        }
    }
}





